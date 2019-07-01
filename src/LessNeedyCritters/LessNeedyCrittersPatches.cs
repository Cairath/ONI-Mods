using Harmony;

namespace LessNeedyCritters
{
    public class LessNeedyCrittersPatches
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

		[HarmonyPatch(typeof(ModifierSet), "LoadEffects")]
	    public static class ModifierSet_LoadEffects_Patch
		{
			public static void Postfix(ref ModifierSet __instance)
		    {
			    __instance.effects.Get("Ranched").duration = 1800f;
		    }
	    }
	}
}
