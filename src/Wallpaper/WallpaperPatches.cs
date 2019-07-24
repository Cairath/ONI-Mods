using System.Collections.Generic;
using Harmony;

namespace Wallpaper
{
	public static class WallpaperPatches
	{
		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				CaiLib.Logger.Logger.LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

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

		[HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
		public static class BuildingComplete_OnSpawn_Patch
		{
			public static void Postfix(BuildingComplete __instance)
			{
				if (__instance.name == "WallpaperComplete")
				{
					var primaryElement = __instance.GetComponent<PrimaryElement>();
					var kAnimBase = __instance.GetComponent<KAnimControllerBase>();

					if (primaryElement != null && kAnimBase != null)
					{
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
		}
	}
}
