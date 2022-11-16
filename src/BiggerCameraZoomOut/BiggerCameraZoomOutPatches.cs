using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;

namespace BiggerCameraZoomOut
{
	public static class BiggerCameraZoomOutPatches
	{
		private static readonly float _maxZoom = 200f;

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

		[HarmonyPatch(typeof(ClusterMapScreen))]
		[HarmonyPatch(nameof(ClusterMapScreen.OnKeyDown))]
		public static class ClusterMapScreen_OnKeyDown_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
			{
				var codes = new List<CodeInstruction>(instructions);
				for (var i = 1; i < codes.Count; i++)
				{

					if (codes[i].opcode == OpCodes.Ldc_R4 && (float) codes[i].operand == 50f)
					{
						codes[i].operand = 20f;
						break;
					}
				}

				return codes.AsEnumerable();
			}
		}
	}
}
