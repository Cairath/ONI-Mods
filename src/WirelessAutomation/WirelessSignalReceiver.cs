using KSerialization;
using UnityEngine;

namespace WirelessAutomation
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class WirelessSignalReceiver : Switch, ISim33ms, IIntSliderControl
	{
		[field: Serialize]
		public int ReceiveChannel { get; set; }

		protected override void OnSpawn()
		{
			base.OnSpawn();
			
			UpdateVisualState(IsSwitchedOn);
			GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, IsSwitchedOn ? 1 : 0); ;
		}

		private void UpdateVisualState(bool isOn)
		{
			GetComponent<KBatchedAnimController>().Play(isOn ? "on_pst" : "off", KAnim.PlayMode.Loop);
		}

		public void Sim33ms(float dt)
		{
			var signal = WirelessAutomationManager.GetSignalForChannel(ReceiveChannel);

			if (IsSwitchedOn != signal)
			{
				ChangeState(signal);
			}
		}

		private void ChangeState(bool isOn)
		{
			UpdateVisualState(isOn);
			GetComponent<LogicPorts>()?.SendSignal(LogicSwitch.PORT_ID, isOn ? 1 : 0);
			Toggle();
		}

		private void ChangeListeningChannel(int channel)
		{
			ReceiveChannel = channel;
		}

		public int SliderDecimalPlaces(int index) => 0;
		public float GetSliderMin(int index) => 0;
		public float GetSliderMax(int index) => 100;
		public float GetSliderValue(int index) => ReceiveChannel;
		public void SetSliderValue(float value, int index) => ChangeListeningChannel(Mathf.RoundToInt(value));
		public string GetSliderTooltipKey(int index) => WirelessAutomationManager.SliderTooltipKey;
		public string SliderTitleKey => WirelessAutomationManager.SliderTitleKey;
		public string SliderUnits => string.Empty;
	}
}
