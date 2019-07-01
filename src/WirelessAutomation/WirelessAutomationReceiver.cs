using KSerialization;
using UnityEngine;

namespace WirelessAutomation
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class WirelessAutomationReceiver : Switch, ISim200ms, IIntSliderControl
	{
		[MyCmpAdd]
		private Operational _operational;

		[Serialize]
		private int _receiveChannel;

		public int ReceiveChannel
		{
			get => _receiveChannel;
			set => _receiveChannel = value;
		}

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

		public void Sim200ms(float dt)
		{
			var signal = WirelessAutomationManager.GetSignalForChannel(_receiveChannel);

			if (IsSwitchedOn != signal)
			{
				ChangeState(signal);
			}
		}

		private void ChangeState(bool isOn)
		{
			UpdateVisualState(isOn);
			var lp = GetComponent<LogicPorts>();
			if (lp == null)
			{
				CaiLib.Logger.Log(ModInfo.Name, "lp null");
			}
			else
			{
				lp.SendSignal(LogicSwitch.PORT_ID, isOn ? 1 : 0);
			}
			Toggle();
		}

		private void ChangeListeningChannel(int channel)
		{
			_receiveChannel = channel;
		}

		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		public float GetSliderMin(int index)
		{
			return 0;
		}

		public float GetSliderMax(int index)
		{
			return 100;
		}

		public float GetSliderValue(int index)
		{
			return _receiveChannel;
		}

		public void SetSliderValue(float value, int index)
		{
			ChangeListeningChannel(Mathf.RoundToInt(value));
		}

		public string GetSliderTooltipKey(int index) => WirelessAutomationManager.SliderTooltipKey;
		public string SliderTitleKey => WirelessAutomationManager.SliderTitleKey;
		public string SliderUnits => string.Empty;
	}
}
