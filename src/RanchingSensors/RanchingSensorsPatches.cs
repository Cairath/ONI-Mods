using CaiLib.Utils;
using Harmony;
using static CaiLib.Logger.Logger;
using static CaiLib.Utils.BuildingUtils;
using static CaiLib.Utils.StringUtils;

namespace RanchingSensors
{
	public static class RanchingSensorsPatches
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
				AddBuildingStrings(CrittersSensorConfig.Id, CrittersSensorConfig.DisplayName, CrittersSensorConfig.Description, CrittersSensorConfig.Effect);
				AddBuildingStrings(EggsSensorConfig.Id, EggsSensorConfig.DisplayName, EggsSensorConfig.Description, EggsSensorConfig.Effect);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Automation, CrittersSensorConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Automation, EggsSensorConfig.Id);
			}

			[HarmonyPatch(typeof(Db))]
			[HarmonyPatch("Initialize")]
			public static class Db_Initialize_Patch
			{
				public static void Prefix()
				{
					AddBuildingToTechnology(GameStrings.Technology.Food.AnimalControl, CrittersSensorConfig.Id);
					AddBuildingToTechnology(GameStrings.Technology.Food.AnimalControl, EggsSensorConfig.Id);
				}
			}
		}
	}
}
