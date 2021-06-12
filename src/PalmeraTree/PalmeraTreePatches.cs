using HarmonyLib;
using static CaiLib.Logger.Logger;
using static CaiLib.Utils.CarePackagesUtils;
using static CaiLib.Utils.PlantUtils;
using static CaiLib.Utils.RecipeUtils;
using static CaiLib.Utils.StringUtils;

namespace PalmeraTree
{
	public class PalmeraTreePatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit();
			}
		}

		[HarmonyPatch(typeof(EntityConfigManager))]
		[HarmonyPatch(nameof(EntityConfigManager.LoadGeneratedEntities))]
		public class EntityConfigManager_LoadGeneratedEntities_Patch
		{
			public static void Prefix()
			{
				AddPlantStrings(PalmeraTreeConfig.Id, PalmeraTreeConfig.Name, PalmeraTreeConfig.Description, PalmeraTreeConfig.DomesticatedDescription);
				AddPlantSeedStrings(PalmeraTreeConfig.Id, PalmeraTreeConfig.SeedName, PalmeraTreeConfig.SeedDescription);
				AddFoodStrings(SteamedPalmeraBerryConfig.Id, SteamedPalmeraBerryConfig.Name, SteamedPalmeraBerryConfig.Description, SteamedPalmeraBerryConfig.RecipeDescription);
				AddFoodStrings(PalmeraBerryConfig.Id, PalmeraBerryConfig.Name, PalmeraBerryConfig.Description);
				AddCropType(PalmeraBerryConfig.Id, 20, 10);
			}
		}

		[HarmonyPatch(typeof(Immigration))]
		[HarmonyPatch("ConfigureCarePackages")]
		public static class Immigration_ConfigureCarePackages_Patch
		{
			public static void Postfix(ref Immigration __instance)
			{
				AddCarePackage(ref __instance, PalmeraTreeConfig.SeedId, 1f, () => CycleCondition(48));
			}
		}

		[HarmonyPatch(typeof(SupermaterialRefineryConfig))]
		[HarmonyPatch("ConfigureBuildingTemplate")]
		public class SupermaterialRefineryConfig_ConfigureBuildingTemplate_Patch
		{
			public static void Postfix()
			{
				AddComplexRecipe(
					input: new[] {
						new ComplexRecipe.RecipeElement(BasicSingleHarvestPlantConfig.SEED_ID.ToTag(), 10f),
						new ComplexRecipe.RecipeElement(BasicFabricConfig.ID.ToTag(), 10f)
					},
					output: new[] { new ComplexRecipe.RecipeElement(TagManager.Create(PalmeraTreeConfig.SeedId), 1f) },
					fabricatorId: SupermaterialRefineryConfig.ID,
					productionTime: 50f,
					recipeDescription: "What will happen if you mash some organic mass together?",
					nameDisplayType: ComplexRecipe.RecipeNameDisplay.Result,
					sortOrder: 1000
				);
			}
		}

		// seems not needed anymore?
		//      [HarmonyPatch(typeof(KSerialization.Manager))]
		//      [HarmonyPatch("GetType")]
		//      [HarmonyPatch(new[] { typeof(string) })]
		//      public static class KSerializationManager_GetType_Patch
		//      {
		//          public static void Postfix(string type_name, ref Type __result)
		//          {
		//              if (type_name == "PalmeraTree.PalmeraTree")
		//              {
		//                  __result = typeof(PalmeraTree);
		//              }
		//          }
		//      }
	}
}
