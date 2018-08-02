using KSerialization;
using STRINGS;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace RanchingSensors
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class CrittersSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
	{
		[SerializeField]
		[Serialize]
		private float threshold;
		[SerializeField]
		[Serialize]
		private bool activateAboveThreshold = true;
		private bool wasOn;
		private int currentCritters = 0;

		public float CurrentValue => currentCritters;
		public LocString Title => "Critters Sensor";
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
			OnToggle += OnSwitchToggled;
			UpdateLogicCircuit();
			UpdateVisualState(true);
			wasOn = IsSwitchedOn;
		}

		public void Sim200ms(float dt)
		{
			var list = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(this)).creatures;
			var selected = new List<KPrefabID>();

			foreach (var prefab in list)
			{
				if (prefab.HasPrefabTag(GameTags.Creature) || prefab.HasPrefabTag(GameTags.Creatures.Burrowed))
				{
					selected.Add(prefab);
				}

			}

			currentCritters = selected.Count;

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

			if (this.wasOn == this.switchedOn && !force)
				return;
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
			component.Play((HashedString)(!this.switchedOn ? "on_pst" : "on_pre"), KAnim.PlayMode.Once, 1f, 0.0f);
			component.Queue((HashedString)(!this.switchedOn ? "off" : "on"), KAnim.PlayMode.Once, 1f, 0.0f);
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