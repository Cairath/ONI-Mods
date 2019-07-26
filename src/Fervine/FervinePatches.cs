using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;
using static CaiLib.Utils.CarePackagesUtils;

namespace Fervine
{
	public class FervinePatches
	{
		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				CaiLib.Logger.Logger.LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(EntityConfigManager))]
		[HarmonyPatch("LoadGeneratedEntities")]
		public static class EntityConfigManager_LoadGeneratedEntities_Patch
		{
			public static void Prefix()
			{
				Strings.Add($"STRINGS.CREATURES.SPECIES.SEEDS.{FervineConfig.Id.ToUpperInvariant()}.NAME",
					FervineConfig.SeedName);
				Strings.Add($"STRINGS.CREATURES.SPECIES.SEEDS.{FervineConfig.Id.ToUpperInvariant()}.DESC",
					FervineConfig.SeedDesc);
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

		[HarmonyPatch(typeof(KSerialization.Manager))]
		[HarmonyPatch("GetType")]
		[HarmonyPatch(new[] { typeof(string) })]
		public static class KSerializationManager_GetType_Patch
		{
			public static void Postfix(string type_name, ref Type __result)
			{
				if (type_name == "Fervine.Fervine")
				{
					__result = typeof(Fervine);
				}
			}
		}

		[HarmonyPatch(typeof(SupermaterialRefineryConfig))]
		[HarmonyPatch("ConfigureBuildingTemplate")]
		public class SupermaterialRefineryConfig_ConfigureBuildingTemplate_Patch
		{
			private const string MolecularForgeId = "SupermaterialRefinery";

			public static void Postfix()
			{
				var ingredients = new[]
				{
					new ComplexRecipe.RecipeElement(SimHashes.Diamond.CreateTag(), 50f),
					new ComplexRecipe.RecipeElement(BasicFabricConfig.ID.ToTag(), 10f)
				};

				var result = new[]
				{
					new ComplexRecipe.RecipeElement(TagManager.Create(FervineConfig.SeedId), 1f)
				};

				new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(MolecularForgeId, ingredients, result), ingredients,
						result)
				{
					time = 50f,
					description = "Plant + shiny = ?",
					useResultAsDescription = true
				}
					.fabricators = new List<Tag>()
				{
					TagManager.Create(MolecularForgeId)
				};
			}
		}
	}
}
