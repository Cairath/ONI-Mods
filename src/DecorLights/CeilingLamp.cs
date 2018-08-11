namespace DecorLights
{
	public class CeilingLamp : GameStateMachine<CeilingLamp, CeilingLamp.Instance>
	{
		public State off;
		public State on;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = off;

			off
				.PlayAnim("misc")
				.EventTransition(GameHashes.OperationalChanged, on, smi => smi.GetComponent<Operational>().IsOperational);
			on
				.Enter("SetActive", smi => smi.GetComponent<Operational>().SetActive(true, false))
				.PlayAnim("on")
				.EventTransition(GameHashes.OperationalChanged, off, smi => !smi.GetComponent<Operational>().IsOperational)
				.ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null);
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