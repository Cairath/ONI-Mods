using Harmony;
using static CaiLib.Logger.Logger;
using static CaiLib.Utils.CarePackagesUtils;
using static CaiLib.Utils.RecipeUtils;
using static CaiLib.Utils.StringUtils;

namespace Fervine
{
	public class FervinePatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit();
			}
		}

		[HarmonyPatch(typeof(EntityConfigManager))]
		[HarmonyPatch("LoadGeneratedEntities")]
		public static class EntityConfigManager_LoadGeneratedEntities_Patch
		{
			public static void Prefix()
			{
				AddPlantStrings(FervineConfig.Id, FervineConfig.Name, FervineConfig.Description, FervineConfig.DomesticatedDescription);
				AddPlantSeedStrings(FervineConfig.Id, FervineConfig.SeedName, FervineConfig.SeedDesc);
			}
		}

		[HarmonyPatch(typeof(Immigration))]
		[HarmonyPatch("ConfigureCarePackages")]
		public static class Immigration_ConfigureCarePackages_Patch
		{
			public static void Postfix(ref Immigration __instance)
			{
				AddCarePackage(ref __instance, FervineConfig.SeedId, 1f);
			}
		}

		[HarmonyPatch(typeof(SupermaterialRefineryConfig))]
		[HarmonyPatch("ConfigureBuildingTemplate")]
		public class SupermaterialRefineryConfig_ConfigureBuildingTemplate_Patch
		{
			public static void Postfix()
			{
				AddComplexRecipe(
					input: new[]
					{
						new ComplexRecipe.RecipeElement(SimHashes.Diamond.CreateTag(), 50f),
						new ComplexRecipe.RecipeElement(BasicFabricConfig.ID.ToTag(), 10f)
					},
					output: new[]
					{
						new ComplexRecipe.RecipeElement(TagManager.Create(FervineConfig.SeedId), 1f)
					},
					fabricatorId: SupermaterialRefineryConfig.ID,
					productionTime: 50f,
					recipeDescription: "Plant + shiny = ?",
					nameDisplayType: ComplexRecipe.RecipeNameDisplay.Result,
					sortOrder: 1000
				);
			}
		}
	}
}
