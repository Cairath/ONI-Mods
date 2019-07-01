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
				CaiLib.Logger.LogInit(ModInfo.Name, ModInfo.Version);
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
		[HarmonyPatch("DoPostConfigureComplete")]
		public static class SolidTransferArmConfig_DoPostConfigureComplete_Patch
		{
			public static void Postfix(ref GameObject go)
			{
				go.AddOrGet<SolidTransferArm>().pickupRange = 8;
			}
		}
	}
}
