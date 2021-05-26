using CaiLib.Utils;
using Harmony;
using static CaiLib.Logger.Logger;
using static CaiLib.Utils.BuildingUtils;
using static CaiLib.Utils.StringUtils;

namespace PipedAlgaeTerrarium
{
	public class PipedAlgaeTerrariumPatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit();
			}
		}

		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				AddBuildingStrings(PipedAlgaeTerrariumConfig.Id, PipedAlgaeTerrariumConfig.DisplayName, PipedAlgaeTerrariumConfig.Description, PipedAlgaeTerrariumConfig.Effect);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Oxygen, PipedAlgaeTerrariumConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Postfix()
			{
				AddBuildingToTechnology(GameStrings.Technology.Food.BasicFarming, PipedAlgaeTerrariumConfig.Id);
			}
		}
	}
}
