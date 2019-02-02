using Harmony;
using UnityEngine;

namespace DoubleSweeperRange
{
	public class DoubleSweeperRangePatches
	{
		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				CaiLib.ModCounter.ModCounter.Hit(ModInfo.Name, ModInfo.Version);
				CaiLib.Logger.LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

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
