using System;
using System.Collections.Generic;
using Harmony;
using KSerialization;

namespace Fervine
{
	public class FervineMod
	{
		[HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
		public class FervineEntityConfigManagerPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.CREATURES.SPECIES.SEEDS.HEATBULB.NAME", FervineConfig.SeedName);
				Strings.Add("STRINGS.CREATURES.SPECIES.SEEDS.HEATBULB.DESC", FervineConfig.SeedDesc);
			}
		}
	}

	[HarmonyPatch(typeof(Manager), "GetType", new[] { typeof(string) })]
	public static class FervineEntitySerializationPatch
	{
		public static void Postfix(string type_name, ref Type __result)
		{
			if (type_name == "Fervine.Fervine")
			{
				__result = typeof(Fervine);
			}
		}
	}

	[HarmonyPatch(typeof(SupermaterialRefineryConfig), "ConfigureBuildingTemplate")]
	public class SupermaterialRefineryConfigPatch
	{
		private static void Postfix()
		{
			var ingredients = new[]
			{
				new ComplexRecipe.RecipeElement(SimHashes.Diamond.CreateTag(), 50f),
				new ComplexRecipe.RecipeElement(BasicFabricConfig.ID.ToTag(), 10f)
			};

			var result = new[]
			{
				new ComplexRecipe.RecipeElement(TagManager.Create(FervineConfig.SEED_ID), 1f)
			};

			new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", ingredients, result), ingredients, result)
			{
				time = 50f,
				description = "Plant + shiny = ?",
				useResultAsDescription = true
			}.fabricators = new List<Tag>()
			{
				TagManager.Create("SupermaterialRefinery")
			};
		}
	}
}