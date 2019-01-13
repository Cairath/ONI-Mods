using Harmony;
using UnityEngine;

namespace DoubleSweeperRange
{
	public class DoubleSweeperRangePatches
	{
		[HarmonyPatch(typeof(SolidTransferArmConfig))]
		[HarmonyPatch("DoPostConfigurePreview")]
		public static class SolidTransferArmConfig_DoPostConfigurePreview_Patch
		{
			public static void Postfix(ref GameObject go)
			{
				go.AddOrGet<StationaryChoreRangeVisualizer>().width = 16;
				go.AddOrGet<StationaryChoreRangeVisualizer>().height = 16;
				go.AddOrGet<StationaryChoreRangeVisualizer>().x = -8;
				go.AddOrGet<StationaryChoreRangeVisualizer>().y = -8;
			}
		}

		[HarmonyPatch(typeof(SolidTransferArmConfig))]
		[HarmonyPatch("DoPostConfigureComplete")]
		public static class SolidTransferArmConfig_DoPostConfigureComplete_Patch
		{
			public static void Postfix(ref GameObject go)
			{
				go.AddOrGet<StationaryChoreRangeVisualizer>().width = 16;
				go.AddOrGet<StationaryChoreRangeVisualizer>().height = 16;
				go.AddOrGet<StationaryChoreRangeVisualizer>().x = -8;
				go.AddOrGet<StationaryChoreRangeVisualizer>().y = -8;
				go.AddOrGet<SolidTransferArm>().pickupRange = 8;
			}
		}
	}
}
