using CaiLib.Utils;
using HarmonyLib;
using static CaiLib.Utils.BuildingUtils;
using static CaiLib.Utils.StringUtils;

namespace Wallpaper
{
	public static class WallpaperPatches
	{
		[HarmonyPatch(typeof(Game))]
		[HarmonyPatch("OnSpawn")]
		public static class Game_OnSpawn_Patch
		{
			public static void Postfix()
			{
				WallpaperMod.ColorRefresher = new ColorRefresher();
				SimAndRenderScheduler.instance.sim1000ms.Add(WallpaperMod.ColorRefresher);
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
			public static void Postfix()
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

				ColorTools.SetColor(__instance);
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

				ColorTools.RecolorWalls();
			}
		}
	}
}
