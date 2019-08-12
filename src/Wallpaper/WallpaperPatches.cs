using System;
using CaiLib.Utils;
using Harmony;
using STRINGS;
using static CaiLib.Logger.Logger;
using static CaiLib.Utils.BuildingUtils;
using static CaiLib.Utils.StringUtils;

namespace Wallpaper
{
	public static class WallpaperPatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				AddBuildingStrings(WallpaperConfig.Id, WallpaperConfig.DisplayName, WallpaperConfig.Description, WallpaperConfig.Effect);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, WallpaperConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				AddBuildingToTechnology(GameStrings.Technology.Decor.ArtisticExpression, WallpaperConfig.Id);
			}
		}

		[HarmonyPatch(typeof(BuildingComplete))]
		[HarmonyPatch("OnSpawn")]
		public static class BuildingComplete_OnSpawn_Patch
		{
			public static void Postfix(BuildingComplete __instance)
			{
				if (__instance.name != "WallpaperComplete") return;

				SetColor(__instance);
			}
		}

		[HarmonyPatch(typeof(OverlayScreen))]
		[HarmonyPatch("ToggleOverlay")]
		public static class OverlayMenu_OnOverlayChanged_Patch
		{
			public static void Postfix(HashedString newMode)
			{
				if (newMode != OverlayModes.None.ID)
					return;

				foreach (var building in Components.BuildingCompletes.Items)
				{

					if (UI.StripLinkFormatting(building.GetProperName()) == WallpaperConfig.DisplayName)
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
