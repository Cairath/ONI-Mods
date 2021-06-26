using HarmonyLib;

namespace NoLeakyWalls
{
	public class NoLeakyWallsPatches
	{
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
