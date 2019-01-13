using Harmony;

namespace DrawBuildingsWithoutMaterials
{
	public class DrawBuildingsWithoutMaterialsPatches
	{
		[HarmonyPatch(typeof(MaterialSelector))]
		[HarmonyPatch("AllowInsufficientMaterialBuild")]
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
