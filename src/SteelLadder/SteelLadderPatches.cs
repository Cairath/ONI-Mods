using CaiLib.Utils;
using HarmonyLib;
using static CaiLib.Utils.BuildingUtils;
using static CaiLib.Utils.GameStrings;

namespace SteelLadder
{
	public static class SteelLadderPatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				StringUtils.AddBuildingStrings(SteelLadderConfig.Id, SteelLadderConfig.DisplayName, SteelLadderConfig.Description, SteelLadderConfig.Effect);
				AddBuildingToPlanScreen(PlanMenuCategory.Base, SteelLadderConfig.Id, "Ladders", LadderFastConfig.ID);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch(nameof(Db.Initialize))]
		public static class Db_Initialize_Patch
		{
			public static void Postfix()
			{
				AddBuildingToTechnology(GameStrings.Technology.Decor.HomeLuxuries, SteelLadderConfig.Id);
			}
		}
	}
}