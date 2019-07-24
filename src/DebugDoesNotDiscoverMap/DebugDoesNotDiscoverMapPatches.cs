using Harmony;

namespace DebugDoesNotDiscoverMap
{
	public class DebugDoesNotDiscoverMapPatches
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

		[HarmonyPatch(typeof(DebugHandler))]
		[HarmonyPatch(nameof(DebugHandler.OnKeyDown))]
		public static class DebugHandler_OnKeyDown
		{
			public static bool Prefix(ref DebugHandler __instance, KButtonEvent e)
			{
				if (!DebugHandler.enabled)
					return false;

				if (!e.TryConsume(Action.DebugToggle)) return true;

				CameraController.Instance.FreeCameraEnabled = !CameraController.Instance.FreeCameraEnabled;

				var activeSelf = DebugPaintElementScreen.Instance.gameObject.activeSelf;
				DebugPaintElementScreen.Instance.gameObject.SetActive(!activeSelf);

				if (DebugElementMenu.Instance && DebugElementMenu.Instance.root.activeSelf)
					DebugElementMenu.Instance.root.SetActive(false);

				DebugBaseTemplateButton.Instance.gameObject.SetActive(!activeSelf);

				return false;
			}
		}
	}
}
