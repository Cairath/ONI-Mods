namespace LightsOut
{
	public class LightsOutMonitor : GameStateMachine<LightsOutMonitor, LightsOutMonitor.Instance>
	{
		public State EnoughLight;
		public State Dark;
		public State PitchBlack;

		public FloatParameter LightLevel;

		public int DarkThreshold = 1000;

		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = EnoughLight;

			root.Update(CheckLightLevel);

			EnoughLight.ParamTransition(LightLevel, Dark, (smi, lux) => lux < DarkThreshold);

			Dark
				.Enter(smi => smi.Effects.Add(Effects.DarkId, false))
				.Exit(smi => smi.Effects.Remove(Effects.DarkId))
				.ParamTransition(LightLevel, PitchBlack, IsZero)
				.ParamTransition(LightLevel, EnoughLight, (smi, lux) => lux >= DarkThreshold);

			PitchBlack
				.Enter(smi => smi.Effects.Add(Effects.PitchBlackId, false))
				.Exit(smi => smi.Effects.Remove(Effects.PitchBlackId))
				.ParamTransition(LightLevel, Dark, IsGTZero);
		}

		private void CheckLightLevel(Instance smi, float dt)
		{
			var cell = Grid.PosToCell(smi.gameObject);

			if (!Grid.IsValidCell(cell))
				return;

			smi.sm.LightLevel.Set(Grid.LightIntensity[cell], smi);
		}

		public new class Instance : GameInstance
		{
			public Klei.AI.Effects Effects;

			public Instance(IStateMachineTarget master) : base(master)
			{
				Effects = GetComponent<Klei.AI.Effects>();
			}
		}
	}
}