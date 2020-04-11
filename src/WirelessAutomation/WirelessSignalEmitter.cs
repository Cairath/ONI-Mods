using KSerialization;
using STRINGS;
using UnityEngine;

namespace WirelessAutomation
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class WirelessSignalEmitter : KMonoBehaviour, IIntSliderControl
	{
		[field: Serialize]
		public int EmitChannel { get; set; }

		[Serialize]
		private int _emitterId;

		[MyCmpGet]
		private LogicPorts _logicPorts;

		protected override void OnPrefabInit()
		{
			Subscribe((int)GameHashes.LogicEvent, OnLogicEventChanged);
		}

		protected override void OnSpawn()
		{
			_emitterId = WirelessAutomationManager.RegisterEmitter(new SignalEmitter(EmitChannel, _logicPorts.GetInputValue(LogicSwitch.PORT_ID)));
		}

		protected override void OnCleanUp()
		{
			Unsubscribe((int)GameHashes.OperationalChanged, OnLogicEventChanged);
			WirelessAutomationManager.UnregisterEmitter(_emitterId);
		}

		private void OnLogicEventChanged(object data)
		{
			var signal = ((LogicValueChanged)data).newValue;

			UpdateVisualState(signal > 0);
			WirelessAutomationManager.SetEmitterSignal(_emitterId, signal);
		}

		private void UpdateVisualState(bool isOn)
		{
			GetComponent<KBatchedAnimController>().Play(isOn ? "on_pst" : "off", KAnim.PlayMode.Loop);
		}

		private void ChangeEmitChannel(int channel)
		{
			EmitChannel = channel;
			WirelessAutomationManager.ChangeEmitterChannel(_emitterId, EmitChannel);
		}

		public int SliderDecimalPlaces(int index) => 0;
		public float GetSliderMin(int index) => 0;
		public float GetSliderMax(int index) => 100;
		public float GetSliderValue(int index) => EmitChannel;
		public void SetSliderValue(float value, int index) => ChangeEmitChannel(Mathf.RoundToInt(value));
		public string GetSliderTooltipKey(int index) => WirelessAutomationManager.SliderTooltipKey;
		public string GetSliderTooltip() => $"Will broadcast received signal on {UI.FormatAsKeyWord($"channel {EmitChannel}")}";
		public string SliderTitleKey => WirelessAutomationManager.SliderTitleKey;
		public string SliderUnits => string.Empty;
	}
}
