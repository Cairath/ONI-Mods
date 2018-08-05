using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace DebugDoesNotDiscoverMap
{
	public class DebugDoesNotDiscoverMapMod
	{
		[HarmonyPatch(typeof(DebugHandler), "OnKeyDown")]
		public static class DebugDoesNotDiscoverMapDebugHandlerPatch
		{
			public static bool Prefix(ref DebugHandler __instance, KButtonEvent e)
			{
				if (!DebugHandler.enabled)
					return false;

				if (e.TryConsume(Action.DebugToggle))
				{
					if ((UnityEngine.Object)CameraController.Instance != null)
						CameraController.Instance.FreeCameraEnabled = !CameraController.Instance.FreeCameraEnabled;

					if ((UnityEngine.Object)DebugPaintElementScreen.Instance != null)
					{
						bool activeSelf = DebugPaintElementScreen.Instance.gameObject.activeSelf;
						DebugPaintElementScreen.Instance.gameObject.SetActive(!activeSelf);
						if (DebugElementMenu.Instance && DebugElementMenu.Instance.root.activeSelf)
							DebugElementMenu.Instance.root.SetActive(false);
						DebugBaseTemplateButton.Instance.gameObject.SetActive(!activeSelf);
					}

					return false;
				}

				return true;
			}

		}
	}
}
