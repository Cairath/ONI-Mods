using CaiLib.Utils;
using Harmony;
using static CaiLib.Logger.Logger;
using static CaiLib.Utils.BuildingUtils;
using static CaiLib.Utils.StringUtils;

namespace AlgaeGrower
{
	public class AlgaeGrowerPatches
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
				AddBuildingStrings(AlgaeGrowerConfig.Id, AlgaeGrowerConfig.DisplayName, AlgaeGrowerConfig.Description, AlgaeGrowerConfig.Effect);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Oxygen, AlgaeGrowerConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				AddBuildingToTechnology(GameStrings.Technology.Food.Agriculture, AlgaeGrowerConfig.Id);
			}
		}
	}
}
