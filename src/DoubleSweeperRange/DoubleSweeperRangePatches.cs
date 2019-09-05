using Harmony;
using UnityEngine;

namespace DoubleSweeperRange
{
	public class DoubleSweeperRangePatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				CaiLib.Logger.Logger.LogInit();
			}
		}

		[HarmonyPatch(typeof(SolidTransferArmConfig))]
		[HarmonyPatch("AddVisualizer")]
		public static class SolidTransferArmConfig_AddVisualizer_Patch
		{
			public static void Postfix(ref GameObject prefab)
			{
				var choreRangeVisualizer = prefab.AddOrGet<StationaryChoreRangeVisualizer>();
				choreRangeVisualizer.x = -8;
				choreRangeVisualizer.y = -8;
				choreRangeVisualizer.width = 17;
				choreRangeVisualizer.height = 17;
			}
		}

		[HarmonyPatch(typeof(SolidTransferArmConfig))]
		[HarmonyPatch(nameof(SolidTransferArmConfig.DoPostConfigureComplete))]
		public static class SolidTransferArmConfig_DoPostConfigureComplete_Patch
		{
			public static void Postfix(ref GameObject go)
			{
				go.AddOrGet<SolidTransferArm>().pickupRange = 8;
			}
		}
	}
}
