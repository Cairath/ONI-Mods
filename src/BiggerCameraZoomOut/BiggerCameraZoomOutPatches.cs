using Harmony;

namespace BiggerCameraZoomOut
{
	public static class BiggerCameraZoomOutPatches
	{
		private static float _maxZoom = 200f;

		[HarmonyPatch(typeof(CameraController), "OnPrefabInit")]
		public static class CameraController_OnPrefabInit_Patch
		{
			public static void Prefix(CameraController __instance)
			{
				Traverse.Create(__instance).Field("maxOrthographicSize").SetValue(_maxZoom);
				CameraController.Instance.FreeCameraEnabled = true;
			}
		}

		[HarmonyPatch(typeof(CameraController))]
		[HarmonyPatch("SetMaxOrthographicSize")]
		public static class CameraController_SetMaxOrthographicSize_Patch
		{
			public static void Prefix(ref float size)
			{
				size = _maxZoom;
			}
		}
	}
}
