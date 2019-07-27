using System;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace RanchingSensors
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class CrittersSensor : Switch, IThresholdSwitch, ISim200ms
	{
		[field: Serialize]
		public float Threshold { get; set; }

		[field: Serialize]
		public bool ActivateAboveThreshold { get; set; } = true;

		public float CurrentValue => _currentCritters;
		public LocString Title => "Live Critters Sensor";
		public LocString ThresholdValueName => UI.CODEX.CATEGORYNAMES.CREATURES;
		public LocString ThresholdValueUnits() => "";
		public string AboveToolTip => "Sensor will be on if the number of critters is above {0}";
		public string BelowToolTip => "Sensor will be on if the number of critters is below {0}";
		public ThresholdScreenLayoutType LayoutType => ThresholdScreenLayoutType.InputField;
		public int IncrementScale => 1;
		public NonLinearSlider.Range[] GetRanges { get; }
		public float RangeMin => 0.0f;
		public float RangeMax => 50.0f;
		public float GetRangeMinInputField() => 0.0f;
		public float GetRangeMaxInputField() => 50.0f;

		private bool _wasOn;
		private int _currentCritters;
		private KSelectable _selectable;
		private Guid _roomStatusGuid;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			_selectable = GetComponent<KSelectable>();
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();
			OnToggle += OnSwitchToggled;
			UpdateLogicCircuit(IsSwitchedOn);
			UpdateVisualState(IsSwitchedOn, true);
			_wasOn = IsSwitchedOn;
		}

		public void Sim200ms(float dt)
		{
			var roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(gameObject);
			if (roomOfGameObject != null)
			{
				_currentCritters = roomOfGameObject.cavity.creatures.Count;

				var newState = ActivateAboveThreshold ? _currentCritters > Threshold : _currentCritters < Threshold;

				SetState(newState);

				if (!_selectable.HasStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom))
					return;
				_selectable.RemoveStatusItem(_roomStatusGuid);
			}
			else
			{
				if (!_selectable.HasStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom))
					_roomStatusGuid = _selectable.AddStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom);

				SetState(false);
			}
		}

		private void OnSwitchToggled(bool toggledOn)
		{
			UpdateLogicCircuit(toggledOn);
			UpdateVisualState(toggledOn);
		}

		private void UpdateVisualState(bool toggledOn, bool force = false)
		{

			if (_wasOn == toggledOn && !force)
				return;
			_wasOn = switchedOn;

			var component = GetComponent<KBatchedAnimController>();
			component.Play(!toggledOn ? "on_pst" : "on_pre");
			component.Queue(!toggledOn ? "off" : "on");
		}

		private void UpdateLogicCircuit(bool toggledOn)
		{
			GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, toggledOn ? 1 : 0);
		}

		public string Format(float value, bool units) => GameUtil.GetFormattedInt(Mathf.RoundToInt(value));
		public float ProcessedSliderValue(float input) => Mathf.RoundToInt(input);
		public float ProcessedInputValue(float input) => Mathf.RoundToInt(input);
	}
}
