using KSerialization;
using STRINGS;
using UnityEngine;

namespace WirelessAutomation
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class WirelessSignalReceiver : KMonoBehaviour, IIntSliderControl
	{
		[field: Serialize]
		public int ReceiveChannel { get; set; }

		[field: Serialize]
		public int Signal { get; set; }

		[Serialize]
		private int _receiverId;

		[MyCmpGet]
		private readonly LogicPorts _logicPorts;

		protected override void OnSpawn()
		{
			_receiverId = WirelessAutomationManager.RegisterReceiver(new SignalReceiver(ReceiveChannel, gameObject));
		
			UpdateVisualState(Signal > 0);
			_logicPorts.SendSignal(LogicSwitch.PORT_ID, Signal);

			Subscribe(WirelessAutomationManager.WirelessLogicEvent, OnWirelessLogicEventChanged);
		}

		private void UpdateVisualState(bool isOn)
		{
			GetComponent<KBatchedAnimController>().Play(isOn ? "on_pst" : "off", KAnim.PlayMode.Loop);
		}

		protected override void OnCleanUp()
		{
			Unsubscribe((int)GameHashes.OperationalChanged, OnWirelessLogicEventChanged);
			WirelessAutomationManager.UnregisterReceiver(_receiverId);
		}

		private void OnWirelessLogicEventChanged(object data)
		{
			var ev = (WirelessLogicValueChanged)data;

			if (Signal != ev.Signal && ev.Channel == ReceiveChannel)
			{
				ChangeState(ev.Signal);
			}
		}

		private void ChangeState(int signal)
		{
			Signal = signal;
			UpdateVisualState(signal > 0);
			_logicPorts?.SendSignal(LogicSwitch.PORT_ID, signal);
		}

		private void ChangeListeningChannel(int channel)
		{
			ReceiveChannel = channel;
			WirelessAutomationManager.ChangeReceiverChannel(_receiverId, ReceiveChannel);
		}

		public int SliderDecimalPlaces(int index) => 0;
		public float GetSliderMin(int index) => 0;
		public float GetSliderMax(int index) => 100;
		public float GetSliderValue(int index) => ReceiveChannel;
		public void SetSliderValue(float value, int index) => ChangeListeningChannel(Mathf.RoundToInt(value));
		public string GetSliderTooltipKey(int index) => WirelessAutomationManager.SliderTooltipKey;
		public string GetSliderTooltip() => $"Will listen to signal broadcast on {UI.PRE_KEYWORD}channel {ReceiveChannel}{UI.PST_KEYWORD}";
		public string SliderTitleKey => WirelessAutomationManager.SliderTitleKey;
		public string SliderUnits => string.Empty;
	}
}
