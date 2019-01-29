using KSerialization;
using UnityEngine;

namespace WirelessAutomation
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class WirelessAutomationEmitter : KMonoBehaviour, IIntSliderControl
	{
		private static readonly EventSystem.IntraObjectHandler<WirelessAutomationEmitter> OnOperationalChangedDelegate = 
			new EventSystem.IntraObjectHandler<WirelessAutomationEmitter>((component, data) => component.OnOperationalChanged(data));

		[MyCmpAdd]
		private Operational _operational;

		[Serialize]
		private int _emitterId;

		[Serialize]
		private int _emitChannel;

		public int EmitChannel
		{
			get => _emitChannel;
			set => _emitChannel = value;
		}

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Subscribe((int)GameHashes.OperationalChanged, OnOperationalChangedDelegate);
		}

		protected override void OnSpawn()
		{
			OnOperationalChanged(_operational.IsOperational);
			base.OnSpawn();
			
			_emitterId = WirelessAutomationManager.RegisterEmitter(new SignalEmitter(_emitChannel, _operational.IsOperational));
		}

		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Unsubscribe((int)GameHashes.OperationalChanged, OnOperationalChangedDelegate, false);	
			WirelessAutomationManager.UnregisterEmitter(_emitterId);
		}

		private void OnOperationalChanged(object data)
		{
			var isOn = (bool) data;
			Debug.Log(isOn);
			
			UpdateVisualState(isOn);
			WirelessAutomationManager.SetEmitterSignal(_emitterId, isOn);
		}

		private void UpdateVisualState(bool isOn)
		{
			GetComponent<KBatchedAnimController>().Play(isOn ? "on_pst" : "off", KAnim.PlayMode.Loop);
		}

		private void ChangeEmitChannel(int channel)
		{
			_emitChannel = channel;
			WirelessAutomationManager.ChangeEmitterChannel(_emitterId, _emitChannel);
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
			return _emitChannel;
		}

		public void SetSliderValue(float value, int index)
		{
			ChangeEmitChannel(Mathf.RoundToInt(value));
		}

		public string GetSliderTooltipKey(int index) => "TOOLTIP";
		public string SliderTitleKey => "CHANNEL";
		public string SliderUnits => string.Empty;
	}
}
