using System.Collections.Generic;
using CaiLib.Utils;
using Harmony;
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
		[HarmonyPatch("LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				AddBuildingStrings(WallpaperConfig.Id, WallpaperConfig.DisplayName, WallpaperConfig.Description, WallpaperConfig.Effect);
				AddBuildingToPlanScreen(GameStrings.BuildingMenuCategory.Furniture, WallpaperConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				AddBuildingToTechnology(GameStrings.Research.Decor.ArtisticExpression, WallpaperConfig.Id);
			}
		}

		[HarmonyPatch(typeof(BuildingComplete))]
		[HarmonyPatch("OnSpawn")]
		public static class BuildingComplete_OnSpawn_Patch
		{
			public static void Postfix(BuildingComplete __instance)
			{
				if (__instance.name != "WallpaperComplete") return;


				var primaryElement = __instance.GetComponent<PrimaryElement>();
				var kAnimBase = __instance.GetComponent<KAnimControllerBase>();
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
}
