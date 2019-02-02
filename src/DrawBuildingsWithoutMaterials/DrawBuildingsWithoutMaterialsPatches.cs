using Harmony;

namespace DrawBuildingsWithoutMaterials
{
	public class DrawBuildingsWithoutMaterialsPatches
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
