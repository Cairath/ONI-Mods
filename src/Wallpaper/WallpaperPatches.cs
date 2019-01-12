using System.Collections.Generic;
using Harmony;

namespace Wallpaper
{
	public static class WallpaperPatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch("LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			private static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WallpaperConfig.Id.ToUpperInvariant()}.NAME", WallpaperConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WallpaperConfig.Id.ToUpperInvariant()}.DESC", WallpaperConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WallpaperConfig.Id.ToUpperInvariant()}.EFFECT", WallpaperConfig.Effect);

				ModUtil.AddBuildingToPlanScreen("Utilities", WallpaperConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			private static void Prefix()
			{
				var luxuryTech = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { WallpaperConfig.Id };
				Database.Techs.TECH_GROUPING["Luxury"] = luxuryTech.ToArray();
			}
		}
	}
}
