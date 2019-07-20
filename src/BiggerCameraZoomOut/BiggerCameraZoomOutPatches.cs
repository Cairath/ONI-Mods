using Harmony;

namespace BiggerCameraZoomOut
{
	public static class BiggerCameraZoomOutPatches
	{
		private static float _maxZoom = 200f;

		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				CaiLib.Logger.LogInit(ModInfo.Name, ModInfo.Version);
			}
		}
		
		[HarmonyPatch(typeof(CameraController), "OnPrefabInit")]
		public static class CameraController_OnPrefabInit_Patch
		{
			public static void Prefix(CameraController __instance)
			{
				Traverse.Create(__instance).Field("maxOrthographicSize").SetValue(_maxZoom);
			}
		}

		[HarmonyPatch(typeof(PlanterBoxConfig), "CreateBuildingDef")]
		public static class PlanterBoxConfigCreateBuildingDef
		{
			public static void Postfix(ref BuildingDef __result)
			{
				__result.AnimFiles = new KAnimFile[]
				{
					Assets.GetAnim("planterbox2")
				};
			}
		}


		[HarmonyPatch(typeof(CameraController))]
		[HarmonyPatch("SetMaxOrthographicSize")]
		public static class CameraController_SetMaxOrthographicSize_Patch
		{
			public static void Prefix(ref float size)
			{
				Debug.Log($"fucking prefix called with size {size}");
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
