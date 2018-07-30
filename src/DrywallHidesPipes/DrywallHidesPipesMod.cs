using Harmony;

namespace DrywallHidesPipes
{
    public class DrywallHidesPipesMod
    {
	    [HarmonyPatch(typeof(ExteriorWallConfig), "CreateBuildingDef")]
	    public static class DrywallHidesPipesPatch
	    {
		    public static void Postfix(ref BuildingDef __result)
		    {
			    __result.SceneLayer = Grid.SceneLayer.WireBridges;
		    }
	    }
	}
}
