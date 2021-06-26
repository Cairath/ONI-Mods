using HarmonyLib;

namespace NoLongCommutes
{
    public class NoLongCommutesPatches
    {
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
