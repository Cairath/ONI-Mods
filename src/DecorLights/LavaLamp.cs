namespace DecorLights
{
	public class LavaLamp : GameStateMachine<LavaLamp, LavaLamp.Instance>
	{
		public State off;
		public State on;
		public State on_pre;
		public State on_pst;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = off;

			off
				.PlayAnim("off")
				.EventTransition(GameHashes.OperationalChanged, on_pre, smi => smi.GetComponent<Operational>().IsOperational);
			on_pre
				.PlayAnim("working_pre")
				.OnAnimQueueComplete(on);
			on
				.Enter("SetActive", smi => smi.GetComponent<Operational>().SetActive(true, false))
				.PlayAnim("working_loop", KAnim.PlayMode.Loop)
				.EventTransition(GameHashes.OperationalChanged, on_pst, smi => !smi.GetComponent<Operational>().IsOperational)
				.ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null);
			on_pst
				.PlayAnim("working_pst")
				.OnAnimQueueComplete(off);


		}

		public class Instance : GameInstance
		{
			public Instance(IStateMachineTarget master)
				: base(master)
			{
			}
		}
	}
}