using HarmonyLib;
using KSerialization;
using STRINGS;
using UnityEngine;

#pragma warning disable 649

namespace ConfigurableMotionSensorRange
{
	public class RangeSwitcher : KMonoBehaviour, ISim200ms, IIntSliderControl
	{
		public static string TooltipKey = "STRINGS.UI.UISIDESCREENS.RANGE_SWITCHER_SIDE_SCREEN.TOOLTIP";
		public static string Tooltip = "Select sensor range";

		public static string TitleKey = "STRINGS.UI.UISIDESCREENS.RANGE_SWITCHER_SIDE_SCREEN.TITLE";
		public static string Title = "Range";

		[field: Serialize]
		public int Range { get; set; }

		[MyCmpGet]
		private readonly LogicDuplicantSensor _logicDuplicantSensor;

		[MyCmpGet]
		private readonly StationaryChoreRangeVisualizer _choreRangeVisualizer;

		[MyCmpGet]
		private readonly Rotatable _rotatable;

		[MyCmpGet]
		private readonly KSelectable _selectable;

		public int SliderDecimalPlaces(int index) => 0;
		public float GetSliderMin(int index) => 3;
		public float GetSliderMax(int index) => 21;
		public float GetSliderValue(int index) => Range;
		public string GetSliderTooltipKey(int index) => TooltipKey;
		public string GetSliderTooltip() => $"Sensor will detect Duplicants within {UI.PRE_KEYWORD}{Range} tiles{UI.PST_KEYWORD}";
		public string SliderTitleKey => TitleKey;
		public string SliderUnits => string.Empty;

		public void SetSliderValue(float value, int index)
		{
			var rounded = Mathf.RoundToInt(value);
			if (rounded % 2 == 0)
				rounded += 1;

			Range = rounded;
		}
	
		public void Sim200ms(float dt)
		{
			if (_choreRangeVisualizer.width == Range)
				return;

			_choreRangeVisualizer.x = -Range / 2;
			_choreRangeVisualizer.y = 0;
			_choreRangeVisualizer.width = Range;
			_choreRangeVisualizer.height = Range;

			if (_selectable.IsSelected)
				Traverse.Create(_choreRangeVisualizer).Method("UpdateVisualizers").GetValue();

			var xy = Grid.CellToXY(this.NaturalBuildingCell());
			var cell = Grid.XYToCell(xy.x, xy.y + Range / 2);
			var offset = new CellOffset(0, Range / 2);

			if (_rotatable)
			{
				var rotatedCellOffset = _rotatable.GetRotatedCellOffset(offset);
				if (Grid.IsCellOffsetValid(this.NaturalBuildingCell(), rotatedCellOffset))
					cell = Grid.OffsetCell(this.NaturalBuildingCell(), rotatedCellOffset);
			}

			var extents = new Extents(cell, Range / 2);

			var sensor = Traverse.Create(_logicDuplicantSensor);
			sensor.Field("pickupableExtents").SetValue(extents);
			sensor.Field("pickupRange").SetValue(Range);

			var handle = sensor.Field("pickupablesChangedEntry").GetValue<HandleVector<int>.Handle>();
			GameScenePartitioner.Instance.Free(ref handle);

			sensor.Field("pickupablesChangedEntry").SetValue(GameScenePartitioner.Instance.Add("DuplicantSensor.PickupablesChanged",
				gameObject, extents, GameScenePartitioner.Instance.pickupablesChangedLayer, Stuff));

			sensor.Field("pickupablesDirty").SetValue(true);

			void Stuff(object data)
			{
				sensor.Method("OnPickupablesChanged", data).GetValue(data);
			}
		}
	}
}
