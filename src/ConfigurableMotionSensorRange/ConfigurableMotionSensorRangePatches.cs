using HarmonyLib;
using UnityEngine;

namespace ConfigurableMotionSensorRange
{
	public class ConfigurableMotionSensorRangePatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{

				Strings.Add(RangeSwitcher.TooltipKey, RangeSwitcher.Tooltip);
				Strings.Add(RangeSwitcher.TitleKey, RangeSwitcher.Title);
			}
		}

		[HarmonyPatch(typeof(LogicDuplicantSensorConfig))]
		[HarmonyPatch(nameof(LogicDuplicantSensorConfig.DoPostConfigureComplete))]
		public static class LogicDuplicantSensorConfig_DoPostConfigureComplete_Patch
		{
			public static void Postfix(GameObject go)
			{
				go.AddComponent<LogicDuplicantSensor>().pickupRange = 5;
				go.AddComponent<RangeSwitcher>().Range = 5;
			}
		}
	}
}
