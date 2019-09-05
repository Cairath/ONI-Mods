using Harmony;
using static CaiLib.Logger.Logger;

namespace DebugDoesNotDiscoverMap
{
	public class DebugDoesNotDiscoverMapPatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit();
			}
		}

		[HarmonyPatch(typeof(DebugHandler))]
		[HarmonyPatch(nameof(DebugHandler.OnKeyDown))]
		public static class DebugHandler_OnKeyDown_Patch
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
