using CaiLib.Utils;
using ConveyorRailUtilities.Filter;
using Harmony;

namespace ConveyorRailUtilities
{
	public static class ConveyorRailUtilitiesPatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				CaiLib.Logger.Logger.LogInit();
			}
		}

		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				StringUtils.AddBuildingStrings(ConveyorFilterConfig.Id, ConveyorFilterConfig.DisplayName,
					ConveyorFilterConfig.Description, ConveyorFilterConfig.Effect);
				BuildingUtils.AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Shipping, ConveyorFilterConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Postfix()
			{
				BuildingUtils.AddBuildingToTechnology(GameStrings.Technology.SolidMaterial.SolidTransport, ConveyorFilterConfig.Id);
			}
		}
	}
}