using System.Collections.Generic;
using Harmony;
using static CaiLib.Logger.Logger;

namespace SteelLadder
{
	public static class SteelLadderPatches
	{
		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
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
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SteelLadderConfig.Id.ToUpperInvariant()}.NAME", SteelLadderConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SteelLadderConfig.Id.ToUpperInvariant()}.DESC", SteelLadderConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SteelLadderConfig.Id.ToUpperInvariant()}.EFFECT", SteelLadderConfig.Effect);

				CaiLib.Utils.BuildingUtils.AddBuildingToPlanScreen("Base", SteelLadderConfig.Id, CarpetTileConfig.ID);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch(nameof(Db.Initialize))]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				CaiLib.Utils.BuildingUtils.AddBuildingToTechnology("Luxury", SteelLadderConfig.Id);
			}
		}
	}
}