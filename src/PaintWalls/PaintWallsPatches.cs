using Harmony;
using static CaiLib.Logger.Logger;

namespace PaintWalls
{
	public static class PaintWallPatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(BuildingComplete))]
		[HarmonyPatch("OnSpawn")]
		public static class BuildingComplete_OnSpawn_Patch
		{
			public static void Postfix(BuildingComplete __instance)
			{
				if (__instance.name == "ExteriorWallComplete" || __instance.name == "ThermalBlockComplete")
				{
					SetColor(__instance);
				}
			}
		}

		[HarmonyPatch(typeof(OverlayScreen))]
		[HarmonyPatch("ToggleOverlay")]
		public static class OverlayMenu_OnOverlayChanged_Patch
		{
			public static void Prefix(HashedString newMode, ref OverlayScreen __instance, out bool __state)
			{
				var val = Traverse.Create(__instance).Field("currentModeInfo").Field("mode").Method("ViewMode").GetValue<HashedString>();

				__state = val == OverlayModes.Decor.ID && newMode != OverlayModes.Decor.ID;
			}

			public static void Postfix(bool __state)
			{
				if (!__state)
				{
					return;
				}

				foreach (var building in Components.BuildingCompletes.Items)
				{

					if (building.name == "ExteriorWallComplete" || building.name == "ThermalBlockComplete")
					{
						SetColor(building);
					}
				}
			}
		}

		private static void SetColor(BuildingComplete building)
		{
			var primaryElement = building.GetComponent<PrimaryElement>();
			var kAnimBase = building.GetComponent<KAnimControllerBase>();
			if (primaryElement == null || kAnimBase == null) return;

			var element = primaryElement.Element;
			var color = element.substance.uiColour;

			if (element.id == SimHashes.Granite)
			{
				color.a = byte.MaxValue;
			}

			kAnimBase.TintColour = color;
		}
	}
}
