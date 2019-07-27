using KSerialization;
using UnityEngine;

namespace WirelessAutomation
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class WirelessSignalEmitter : KMonoBehaviour, IIntSliderControl
	{
		[MyCmpAdd]
#pragma warning disable 649
		private Operational operational;
#pragma warning restore 649

		[Serialize]
		private int _emitterId;

		[field: Serialize]
		public int EmitChannel { get; set; }

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Subscribe((int)GameHashes.OperationalChanged, OnOperationalChanged);
		}

		protected override void OnSpawn()
		{
			OnOperationalChanged(operational.IsOperational);
			base.OnSpawn();

			_emitterId = WirelessAutomationManager.RegisterEmitter(new SignalEmitter(EmitChannel, operational.IsOperational));
		}

		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Unsubscribe((int)GameHashes.OperationalChanged, OnOperationalChanged);
			WirelessAutomationManager.UnregisterEmitter(_emitterId);
		}

		private void OnOperationalChanged(object data)
		{
			var isOn = (bool)data;

			UpdateVisualState(isOn);
			WirelessAutomationManager.SetEmitterSignal(_emitterId, isOn);
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
		public string SliderTitleKey => WirelessAutomationManager.SliderTitleKey;
		public string SliderUnits => string.Empty;
	}
}
