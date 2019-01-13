using Harmony;

namespace LessNeedyCritters
{
    public class LessNeedyCrittersPatches
    {
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
