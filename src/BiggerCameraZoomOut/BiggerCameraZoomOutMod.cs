using Harmony;

namespace BiggerCameraZoomOut
{
    public class BiggerCameraZoomOutMod
    {
	    [HarmonyPatch(typeof(CameraController), "OnPrefabInit")]
	    public static class CameraControllerMod
	    {
		    public static void Prefix(CameraController __instance)
		    {
			    AccessTools.Field(typeof(CameraController), "maxOrthographicSize").SetValue(__instance, 100f);
		    }
	    }

	    [HarmonyPatch(typeof(CameraController), nameof(CameraController.SetMaxOrthographicSize))]
	    public static class CameraControllerMod2
	    {
		    public static void Prefix(CameraController __instance, ref float size)
		    {
			    size = 100f;
		    }
	    }
	}
}
