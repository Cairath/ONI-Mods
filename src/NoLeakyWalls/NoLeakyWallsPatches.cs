using Harmony;
using static CaiLib.Logger.Logger;

namespace NoLeakyWalls
{
	public class NoLeakyWallsPatches
	{
		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(ExteriorWallConfig))]
		[HarmonyPatch(nameof(ExteriorWallConfig.CreateBuildingDef))]
		public static class ExteriorWallConfig_CreateBuildingDef_Patch
		{
			public static void Postfix(ref BuildingDef __result)
			{
				__result.BuildLocationRule = BuildLocationRule.Anywhere;
			}
		}

		[HarmonyPatch(typeof(ThermalBlockConfig))]
		[HarmonyPatch(nameof(ThermalBlockConfig.CreateBuildingDef))]
		public static class ThermalBlockConfig_CreateBuildingDef_Patch
		{
			public static void Postfix(ref BuildingDef __result)
			{
				__result.BuildLocationRule = BuildLocationRule.Anywhere;
			}
		}
	}
}
