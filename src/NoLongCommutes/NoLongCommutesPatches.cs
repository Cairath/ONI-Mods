using HarmonyLib;
using static CaiLib.Logger.Logger;

namespace NoLongCommutes
{
    public class NoLongCommutesPatches
    {
	    public static class Mod_OnLoad
		{
			public static void OnLoad()
		    {
			    LogInit();
		    }
	    }

	    [HarmonyPatch(typeof(Tutorial))]
	    [HarmonyPatch("LongTravelTimes")]
	    public class Tutorial_LongTravelTimes_Patch
		{
		    public static bool Prefix(ref bool __result)
		    {
			    __result = true;
			    return false;
		    }
	    }
	}
}
