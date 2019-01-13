using KSerialization;
using STRINGS;
using UnityEngine;

namespace RanchingSensors
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class EggsSensor : Switch, IThresholdSwitch, ISim200ms
	{
		[SerializeField]
		[Serialize]
		private float _threshold;

		[SerializeField]
		[Serialize]
		private bool _activateAboveThreshold = true;

		private bool _wasOn;
		private int _currentEggs;

		public float CurrentValue => _currentEggs;
		public LocString Title => "Eggs Sensor";
		public LocString ThresholdValueName => UI.CODEX.CATEGORYNAMES.CREATURES;
		public LocString ThresholdValueUnits() => "";
		public string AboveToolTip => "Sensor will be on if the number of eggs is above {0}";
		public string BelowToolTip => "Sensor will be on if the number of eggs is below {0}";
		public ThresholdScreenLayoutType LayoutType => ThresholdScreenLayoutType.InputField;
		public int IncrementScale => 1;
		public NonLinearSlider.Range[] GetRanges { get; }
		public float RangeMin => 0.0f;
		public float RangeMax => 50.0f;
		public float GetRangeMinInputField() => 0.0f;
		public float GetRangeMaxInputField() => 50.0f;

		protected override void OnSpawn()
		{
			base.OnSpawn();
			OnToggle += OnSwitchToggled;
			UpdateLogicCircuit();
			UpdateVisualState(true);
			_wasOn = IsSwitchedOn;
		}

		public void Sim200ms(float dt)
		{
			_currentEggs = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(this)).eggs.Count;

			if (_activateAboveThreshold)
			{
				if (_currentEggs > _threshold && !IsSwitchedOn)
				{
					Toggle();
				}
				else if (_currentEggs <= _threshold && IsSwitchedOn)
				{
					Toggle();
				}
			}
			else if (!_activateAboveThreshold)
			{
				if (_currentEggs < _threshold && !IsSwitchedOn)
				{
					Toggle();
				}
				else if (_currentEggs >= _threshold && IsSwitchedOn)
				{
					Toggle();
				}
			}
		}

		private void OnSwitchToggled(bool toggledOn)
		{
			UpdateLogicCircuit();
			UpdateVisualState();
		}

		private void UpdateVisualState(bool force = false)
		{

			if (_wasOn == switchedOn && !force)
				return;
			_wasOn = switchedOn;

			var component = GetComponent<KBatchedAnimController>();
			component.Play(!switchedOn ? "on_pst" : "on_pre");
			component.Queue(!switchedOn ? "off" : "on");
		}

		public float Threshold
		{
			get => _threshold;
			set => _threshold = value;
		}

		public bool ActivateAboveThreshold
		{
			get => _activateAboveThreshold;
			set => _activateAboveThreshold = value;
		}

		private void UpdateLogicCircuit()
		{
			GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, switchedOn ? 1 : 0);
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
