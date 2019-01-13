namespace DecorLights
{
	public class LavaLamp : GameStateMachine<LavaLamp, LavaLamp.Instance>
	{
		public State Off;
		public State On;
		public State OnPre;
		public State OnPst;

		public override void InitializeStates(out BaseState defaultState)
		{
			defaultState = Off;

			Off
				.PlayAnim("off")
				.EventTransition(GameHashes.OperationalChanged, OnPre, smi => smi.GetComponent<Operational>().IsOperational);

			OnPre
				.PlayAnim("working_pre")
				.OnAnimQueueComplete(On);

			On
				.Enter("SetActive", smi => smi.GetComponent<Operational>().SetActive(true))
				.PlayAnim("working_loop", KAnim.PlayMode.Loop)
				.EventTransition(GameHashes.OperationalChanged, OnPst, smi => !smi.GetComponent<Operational>().IsOperational)
				.ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null);

			OnPst
				.PlayAnim("working_pst")
				.OnAnimQueueComplete(Off);
		}

		public new class Instance : GameInstance
		{
			public Instance(IStateMachineTarget master) : base(master) { }
		}
	}
}
