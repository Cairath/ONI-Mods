using KSerialization;

namespace WirelessAutomation
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class WirelessAutomationEmitter : KMonoBehaviour
	{
		private static readonly EventSystem.IntraObjectHandler<WirelessAutomationEmitter> OnOperationalChangedDelegate = 
			new EventSystem.IntraObjectHandler<WirelessAutomationEmitter>((component, data) => component.OnOperationalChanged(data));

		[MyCmpAdd]
		private Operational _operational;

		[Serialize]
		private int _emitterId;

		[Serialize]
		private int _emitChannel;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Subscribe((int)GameHashes.OperationalChanged, OnOperationalChangedDelegate);
		}

		protected override void OnSpawn()
		{
			OnOperationalChanged(_operational.IsOperational);
			base.OnSpawn();

			_emitChannel = 0;
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
	}
}
