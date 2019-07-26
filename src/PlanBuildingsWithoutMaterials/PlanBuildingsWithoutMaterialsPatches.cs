using Harmony;
using static CaiLib.Logger.Logger;

namespace PlanBuildingsWithoutMaterials
{
	public class PlanBuildingsWithoutMaterialsPatches
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
