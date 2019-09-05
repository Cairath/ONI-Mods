using Harmony;
using static CaiLib.Logger.Logger;

namespace BiggerCameraZoomOut
{
	public static class BiggerCameraZoomOutPatches
	{
		private static readonly float _maxZoom = 200f;

		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit();
			}
		}
		
		[HarmonyPatch(typeof(CameraController))]
		[HarmonyPatch("OnPrefabInit")]
		public static class CameraController_OnPrefabInit_Patch
		{
			public static void Prefix(CameraController __instance)
			{
				Traverse.Create(__instance).Field("maxOrthographicSize").SetValue(_maxZoom);
			}
		}

		[HarmonyPatch(typeof(CameraController))]
		[HarmonyPatch(nameof(CameraController.SetMaxOrthographicSize))]
		public static class CameraController_SetMaxOrthographicSize_Patch
		{
			public static void Prefix(ref float size)
			{
				size = _maxZoom;
			}
		}

		[HarmonyPatch(typeof(CameraController))]
		[HarmonyPatch("ConstrainToWorld")]
		public static class CameraController_ConstrainToWorld_Patch
		{
			public static bool Prefix()
			{
				return false;
			}
		}

		[HarmonyPatch(typeof(WattsonMessage))]
		[HarmonyPatch("OnDeactivate")]
		public static class WattsonMessage_OnDeactivate_Patch
		{
			public static void Postfix()
			{
				UIScheduler.Instance?.Schedule("zoomConfig", 0.7f,
					data => CameraController.Instance.SetMaxOrthographicSize(_maxZoom));
			}
		}
	}
}
