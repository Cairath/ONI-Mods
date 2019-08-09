namespace DecorLights
{
	public class CeilingLamp : GameStateMachine<CeilingLamp, CeilingLamp.Instance>
	{
		public State Off;
		public State On;

		public override void InitializeStates(out BaseState defaultState)
		{
			defaultState = Off;

			Off
				.PlayAnim("misc")
				.EventTransition(GameHashes.OperationalChanged, On, smi => smi.GetComponent<Operational>().IsOperational);
			On
				.Enter("SetActive", smi => smi.GetComponent<Operational>().SetActive(true))
				.PlayAnim("on")
				.EventTransition(GameHashes.OperationalChanged, Off, smi => !smi.GetComponent<Operational>().IsOperational)
				.ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null);
		}

		public new class Instance : GameInstance
		{
			public Instance(IStateMachineTarget master) : base(master) { }
		}
	}
}
