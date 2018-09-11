using Harmony;

namespace LessNeedyCritters
{
    public class LessNeedyCrittersMod
    {
	    [HarmonyPatch(typeof(ModifierSet), "LoadEffects")]
	    public class ModifierSetPatch
		{
		    private static void Postfix(ref ModifierSet __instance)
		    {
			    __instance.effects.Get("Ranched").duration = 1800f;
		    }
	    }
	}
}
