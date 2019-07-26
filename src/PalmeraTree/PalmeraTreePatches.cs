using System;
using System.Collections.Generic;
using Harmony;
using TUNING;
using UnityEngine;

namespace PalmeraTree
{
    public class PalmeraTreePatches
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

        [HarmonyPatch(typeof(CookingStationConfig))]
        [HarmonyPatch("ConfigureBuildingTemplate")]
        public class CookingStationConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go)
            {
                var storedItemModifiers = new List<Storage.StoredItemModifier> { Storage.StoredItemModifier.Hide, Storage.StoredItemModifier.Seal };

                var station = go.GetComponent<CookingStation>();
                station.inStorage.SetDefaultStoredItemModifiers(storedItemModifiers);
                station.buildStorage.SetDefaultStoredItemModifiers(storedItemModifiers);
                station.outStorage.SetDefaultStoredItemModifiers(storedItemModifiers);

                //todo: change cooking station
            }
        }

        [HarmonyPatch(typeof(EntityConfigManager))]
        [HarmonyPatch(nameof(EntityConfigManager.LoadGeneratedEntities))]
        public class EntityConfigManager_LoadGeneratedEntities_Patch
        {
            public static void Prefix()
            {
                Strings.Add($"STRINGS.CREATURES.SPECIES.SEEDS.{PalmeraTreeConfig.Id.ToUpperInvariant()}.NAME", PalmeraTreeConfig.SeedName);
                Strings.Add($"STRINGS.CREATURES.SPECIES.SEEDS.{PalmeraTreeConfig.Id.ToUpperInvariant()}.DESC", PalmeraTreeConfig.SeedDescription);

                Strings.Add($"STRINGS.CREATURES.SPECIES.{PalmeraTreeConfig.Id.ToUpperInvariant()}.NAME", PalmeraTreeConfig.Name);
                Strings.Add($"STRINGS.CREATURES.SPECIES.{PalmeraTreeConfig.Id.ToUpperInvariant()}.DESC", PalmeraTreeConfig.Description);
                Strings.Add($"STRINGS.CREATURES.SPECIES.{PalmeraTreeConfig.Id.ToUpperInvariant()}.DOMESTICATEDDESC", PalmeraTreeConfig.DomesticatedDescription);

                Strings.Add($"STRINGS.ITEMS.FOOD.{SteamedPalmeraBerryConfig.Id.ToUpperInvariant()}.NAME", SteamedPalmeraBerryConfig.Name);
                Strings.Add($"STRINGS.ITEMS.FOOD.{SteamedPalmeraBerryConfig.Id.ToUpperInvariant()}.DESC", SteamedPalmeraBerryConfig.Description);
                Strings.Add($"STRINGS.ITEMS.FOOD.{SteamedPalmeraBerryConfig.Id.ToUpperInvariant()}.RECIPEDESC", SteamedPalmeraBerryConfig.RecipeDescription);

                Strings.Add($"STRINGS.ITEMS.FOOD.{PalmeraBerryConfig.Id.ToUpperInvariant()}.NAME", PalmeraBerryConfig.Name);
                Strings.Add($"STRINGS.ITEMS.FOOD.{PalmeraBerryConfig.Id.ToUpperInvariant()}.DESC", PalmeraBerryConfig.Description);

                CROPS.CROP_TYPES.Add(new Crop.CropVal(PalmeraBerryConfig.Id, 12000f, 10));
            }
        }

        [HarmonyPatch(typeof(SupermaterialRefineryConfig))]
        [HarmonyPatch("ConfigureBuildingTemplate")]
        public class SupermaterialRefineryConfig_ConfigureBuildingTemplate_Patch
        {
            private const string MolecularForgeId = SupermaterialRefineryConfig.ID;

            public static void Postfix()
            {
                var ingredients = new[]
                {
                    new ComplexRecipe.RecipeElement("BasicSingleHarvestPlantSeed", 10f),
                    new ComplexRecipe.RecipeElement(BasicFabricConfig.ID.ToTag(), 10f)
                };

                var result = new[]
                {
                    new ComplexRecipe.RecipeElement(TagManager.Create(PalmeraTreeConfig.SeedId), 1f)
                };

                var recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(MolecularForgeId, ingredients, result), ingredients, result)
                {
                    time = 50f,
                    description = "What will happen if you mash some organic mass together?",
                    nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
                    fabricators = new List<Tag> { MolecularForgeId }
                };
            }
        }

        [HarmonyPatch(typeof(KSerialization.Manager))]
        [HarmonyPatch("GetType")]
        [HarmonyPatch(new[] { typeof(string) })]
        public static class KSerializationManager_GetType_Patch
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
