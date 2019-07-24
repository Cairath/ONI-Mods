using System.Collections.Generic;
using Harmony;
using TUNING;
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
			private static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SteelLadderConfig.Id.ToUpperInvariant()}.NAME", SteelLadderConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SteelLadderConfig.Id.ToUpperInvariant()}.DESC", SteelLadderConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SteelLadderConfig.Id.ToUpperInvariant()}.EFFECT", SteelLadderConfig.Effect);

				AddBuildingToPlanScreen("Base", SteelLadderConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch(nameof(Db.Initialize))]
		public static class Db_Initialize_Patch
		{
			private static void Prefix()
			{
				var luxuryTech = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { SteelLadderConfig.Id };
				Database.Techs.TECH_GROUPING["Luxury"] = luxuryTech.ToArray();
			}
		}

		private static void AddBuildingToPlanScreen(HashedString category, string buildingId, string addAfterBuildingId = null)
		{
			var index = BUILDINGS.PLANORDER.FindIndex(x => x.category == category);

			if (index == -1)
				return;

			var basePlanOrderList = BUILDINGS.PLANORDER[index].data as IList<string>;
			if (basePlanOrderList == null)
			{
				Log(ModInfo.Name, "Could not add Steel Ladder to the building menu.");
				return;
			}

			var carpetIdx = basePlanOrderList.IndexOf(LadderFastConfig.ID);
			basePlanOrderList.Insert(carpetIdx + 1, buildingId);
		}
	}
}