using HarmonyLib;
using static CaiLib.Logger.Logger;

namespace NoLeakyWalls
{
	public class NoLeakyWallsPatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit();
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
