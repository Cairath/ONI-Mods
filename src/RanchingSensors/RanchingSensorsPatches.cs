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
		[HarmonyPatch("LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				AddBuildingStrings(CrittersSensorConfig.Id, CrittersSensorConfig.DisplayName, CrittersSensorConfig.Description, CrittersSensorConfig.Effect);
				AddBuildingStrings(EggsSensorConfig.Id, EggsSensorConfig.DisplayName, EggsSensorConfig.Description, EggsSensorConfig.Effect);
				AddBuildingToPlanScreen(GameStrings.BuildingMenuCategory.Automation, CrittersSensorConfig.Id);
				AddBuildingToPlanScreen(GameStrings.BuildingMenuCategory.Automation, EggsSensorConfig.Id);
			}

			[HarmonyPatch(typeof(Db))]
			[HarmonyPatch("Initialize")]
			public static class Db_Initialize_Patch
			{
				public static void Prefix()
				{
					AddBuildingToTechnology(GameStrings.Research.Food.AnimalControl, CrittersSensorConfig.Id);
					AddBuildingToTechnology(GameStrings.Research.Food.AnimalControl, EggsSensorConfig.Id);
				}
			}
		}
	}
}
