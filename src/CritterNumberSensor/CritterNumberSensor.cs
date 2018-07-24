using KSerialization;
using STRINGS;
using UnityEngine;

namespace CritterNumberSensor
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class CritterNumberSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
	{
		private static readonly HashedString[] ON_ANIMS = new HashedString[2] { "on_pre", "on_loop" };
		private static readonly HashedString[] OFF_ANIMS = new HashedString[2] { "on_pst", "off" };

		[SerializeField]
		[Serialize]
		private float threshold;
		[SerializeField]
		[Serialize]
		private bool activateAboveThreshold = true;
		private bool wasOn;
		private KBatchedAnimController animController;
		private int currentCritters = 0;

		public float CurrentValue => currentCritters;
		public LocString Title => "Critter Number Sensor";
		public LocString ThresholdValueName => UI.CODEX.CATEGORYNAMES.CREATURES;
		public LocString ThresholdValueUnits() => "";
		public string AboveToolTip => "Sensor will be on if the number of critters is above {0}";
		public string BelowToolTip => "Sensor will be on if the number of critters is below {0}";
		public float RangeMin => 0.0f;
		public float RangeMax => 50.0f;
		public float GetRangeMinInputField() => 0.0f;
		public float GetRangeMaxInputField() => 50.0f;

		protected override void OnSpawn()
		{
			base.OnSpawn();
			animController = GetComponent<KBatchedAnimController>();
			OnToggle += OnSwitchToggled;
			UpdateLogicCircuit();
			UpdateVisualState(true);
			wasOn = IsSwitchedOn;
		}

		public void Sim200ms(float dt)
		{
			currentCritters = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(this)).creatures.Count;

			if (activateAboveThreshold)
			{
				if (currentCritters > threshold && !IsSwitchedOn)
				{
					Toggle();
				}
				else if (currentCritters <= threshold && IsSwitchedOn)
				{
					Toggle();
				}
			}
			else if (!activateAboveThreshold)
			{
				if (currentCritters < threshold && !IsSwitchedOn)
				{
					Toggle();
				}
				else if (currentCritters >= threshold && IsSwitchedOn)
				{
					Toggle();
				}
			}
		}

		private void OnSwitchToggled(bool toggled_on)
		{
			UpdateLogicCircuit();
			UpdateVisualState();
		}

		private void UpdateVisualState(bool force = false)
		{
			if (wasOn == switchedOn && !force)
				return;

			wasOn = switchedOn;

			if (switchedOn)
				animController.Play(ON_ANIMS, KAnim.PlayMode.Loop);	
			else
				animController.Play(OFF_ANIMS, KAnim.PlayMode.Once);
		}

		public float Threshold
		{
			get => threshold;
			set => threshold = value;
		}

		public bool ActivateAboveThreshold
		{
			get => activateAboveThreshold;
			set => activateAboveThreshold = value;
		}

		private void UpdateLogicCircuit()
		{
			GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, !switchedOn ? 0 : 1);
		}	

		public string Format(float value, bool units)
		{
			return GameUtil.GetFormattedInt(Mathf.RoundToInt(value));
		}

		public float ProcessedSliderValue(float input)
		{
			return Mathf.RoundToInt(input);
		}

		public float ProcessedInputValue(float input)
		{
			return Mathf.RoundToInt(input);
		}
	}
}