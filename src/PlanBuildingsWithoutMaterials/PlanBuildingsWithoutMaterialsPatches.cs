using HarmonyLib;

namespace PlanBuildingsWithoutMaterials
{
	public class PlanBuildingsWithoutMaterialsPatches
	{
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
