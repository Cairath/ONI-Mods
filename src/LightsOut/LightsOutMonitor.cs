namespace LightsOut
{
	public class LightsOutMonitor : GameStateMachine<LightsOutMonitor, LightsOutMonitor.Instance>
	{
		public State EnoughLight;
		public State Dark;
		public State PitchBlack;

		public FloatParameter LightLevel;

		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			var darkThreshold = LightsOutPatches.ConfigManager.Config.DarkLuxThreshold;
			var pitchBlackTreshold = LightsOutPatches.ConfigManager.Config.PitchBlackLuxThreshold;

			default_state = EnoughLight;

			root.Update(CheckLightLevel);

			EnoughLight.ParamTransition(LightLevel, Dark, (smi, lux) => lux < darkThreshold);

			Dark
				.Enter(smi => smi.Effects.Add(Effects.DarkId, false))
				.Exit(smi => smi.Effects.Remove(Effects.DarkId))
				.ParamTransition(LightLevel, PitchBlack, (smi, lux) => lux <= pitchBlackTreshold)
				.ParamTransition(LightLevel, EnoughLight, (smi, lux) => lux >= darkThreshold);

			PitchBlack
				.Enter(smi => smi.Effects.Add(Effects.PitchBlackId, false))
				.Exit(smi => smi.Effects.Remove(Effects.PitchBlackId))
				.ParamTransition(LightLevel, Dark, (smi, lux) => lux > pitchBlackTreshold);
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