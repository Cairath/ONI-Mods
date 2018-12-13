using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;
using KSerialization;
using TUNING;

namespace PalmeraTree
{
	public class PalmeraTreeMod
	{
		[HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
		public class PalmeraTreeEntityConfigManagerPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.TRELLIS.NAME", "Trellis");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.TRELLIS.DESC", "Used to plant trees.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.TRELLIS.EFFECT", "For when you want to grow your very own tree.");

				Strings.Add("STRINGS.CREATURES.SPECIES.SEEDS.PALMERATREEPLANT.NAME", PalmeraTreeConfig.SeedName);
				Strings.Add("STRINGS.CREATURES.SPECIES.SEEDS.PALMERATREEPLANT.DESC", PalmeraTreeConfig.SeedDesc);

				Strings.Add("STRINGS.CREATURES.SPECIES.PALMERATREEPLANT.NAME", PalmeraTreeConfig.Name);
				Strings.Add("STRINGS.CREATURES.SPECIES.PALMERATREEPLANT.DESC", PalmeraTreeConfig.Desc);
				Strings.Add("STRINGS.CREATURES.SPECIES.PALMERATREEPLANT.DOMESTICATEDDESC", PalmeraTreeConfig.DomesticatedDesc);

				Strings.Add("STRINGS.ITEMS.FOOD." + SteamedPalmeraBerryConfig.ID.ToUpper() + ".NAME",
					SteamedPalmeraBerryConfig.NameStr);
				Strings.Add("STRINGS.ITEMS.FOOD." + SteamedPalmeraBerryConfig.ID.ToUpper() + ".DESC",
					SteamedPalmeraBerryConfig.Desc);
				Strings.Add("STRINGS.ITEMS.FOOD." + SteamedPalmeraBerryConfig.ID.ToUpper() + ".RECIPEDESC",
					SteamedPalmeraBerryConfig.RecipeDesc);

				Strings.Add("STRINGS.ITEMS.FOOD." + PalmeraBerryConfig.ID.ToUpper() + ".NAME", PalmeraBerryConfig.NameStr);
				Strings.Add("STRINGS.ITEMS.FOOD." + PalmeraBerryConfig.ID.ToUpper() + ".DESC", PalmeraBerryConfig.Desc);

				ModUtil.AddBuildingToPlanScreen("Food", TrellisConfig.ID);

				CROPS.CROP_TYPES.Add(new Crop.CropVal(PalmeraBerryConfig.ID, 12000f, 10));
			}
		}

		[HarmonyPatch(typeof(SupermaterialRefineryConfig), "ConfigureBuildingTemplate")]
		public class SupermaterialRefineryConfigPatch
		{
			private static void Postfix()
			{
				var ingredients = new[]
				{
					new ComplexRecipe.RecipeElement(TagManager.Create("BasicSingleHarvestPlantSeed"), 10f),
					new ComplexRecipe.RecipeElement(BasicFabricConfig.ID.ToTag(), 10f)
				};

				var result = new[]
				{
					new ComplexRecipe.RecipeElement(TagManager.Create(PalmeraTreeConfig.SEED_ID), 1f)
				};

				new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", ingredients, result), ingredients,
					result)
				{
					time = 50f,
					description = "What will happen if you mash some organic mass together?",
					useResultAsDescription = true
				}.fabricators = new List<Tag>()
				{
					TagManager.Create("SupermaterialRefinery")
				};
			}
		}

		[HarmonyPatch(typeof(BuildingLoader), "Add2DComponents")]
		public class Add2DComponentsPatch
		{
			private static void Prefix(ref string initialAnimState, BuildingDef def)
			{
				if (initialAnimState == "place" && def.Name.ToLower().Contains("trellis"))
				{
					initialAnimState = "place_1";
				}
				
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class PalmeraTreeDbPatch
		{
			private static void Prefix()
			{
				List<string> tech = new List<string>(Database.Techs.TECH_GROUPING["FarmingTech"]) {TrellisConfig.ID};
				Database.Techs.TECH_GROUPING["FarmingTech"] = tech.ToArray();
			}
		}

		[HarmonyPatch(typeof(Manager), "GetType", new[] {typeof(string)})]
		public static class PalmeraTreeEntitySerializationPatch
		{
			public static void Postfix(string type_name, ref Type __result)
			{
				if (type_name == "PalmeraTree.PalmeraTree")
				{
					__result = typeof(PalmeraTree);
				}
			}
		}
	}
}