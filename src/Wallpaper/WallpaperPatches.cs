using System.Collections.Generic;
using Harmony;

namespace Wallpaper
{
	public class WallpaperPatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class WallpaperGeneratedBuildingsLoadGeneratedBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WallpaperConfig.Id.ToUpperInvariant()}.NAME", WallpaperConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WallpaperConfig.Id.ToUpperInvariant()}.DESC", WallpaperConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WallpaperConfig.Id.ToUpperInvariant()}.EFFECT", WallpaperConfig.Effect);

				ModUtil.AddBuildingToPlanScreen("Utilities", WallpaperConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class WallpaperDbInitializePatch
		{
			private static void Prefix()
			{
				var luxuryTech = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { WallpaperConfig.Id };
				Database.Techs.TECH_GROUPING["Luxury"] = luxuryTech.ToArray();
			}
		}
	}
}
