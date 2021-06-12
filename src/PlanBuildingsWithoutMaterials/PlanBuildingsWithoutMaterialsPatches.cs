using HarmonyLib;
using static CaiLib.Logger.Logger;

namespace PlanBuildingsWithoutMaterials
{
	public class PlanBuildingsWithoutMaterialsPatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit();
			}
		}

		[HarmonyPatch(typeof(MaterialSelector))]
		[HarmonyPatch(nameof(MaterialSelector.AllowInsufficientMaterialBuild))]
		public static class MaterialSelector_AllowInsufficientMaterialBuild_Patch
		{
			public static bool Prefix(ref bool __result)
			{
				__result = true;
				return false;
			}
		}
	}
}
