using Harmony;
using UnityEngine;

namespace DoubleSweeperRange
{
	public class DoubleSweeperRangeMod
	{
		[HarmonyPatch(typeof(SolidTransferArmConfig), "DoPostConfigurePreview")]
		public class DoPostConfigurePreviewPatch
		{
			private static void Postfix(ref GameObject go)
			{
				go.AddOrGet<StationaryChoreRangeVisualizer>().width = 16;
				go.AddOrGet<StationaryChoreRangeVisualizer>().height = 16;
				go.AddOrGet<StationaryChoreRangeVisualizer>().x = -8;
				go.AddOrGet<StationaryChoreRangeVisualizer>().y = -8;
			}
		}

		[HarmonyPatch(typeof(SolidTransferArmConfig), "DoPostConfigureComplete")]
		public class DoPostConfigureCompletePatch
		{
			private static void Postfix(ref GameObject go)
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
