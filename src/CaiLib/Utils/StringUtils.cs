using STRINGS;

namespace CaiLib.Utils
{
    public static class StringUtils
    {
        public static void AddBuildingStrings(string buildingId, string name, string description, string effect)
        {
            Strings.Add($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.NAME", UI.FormatAsLink(name, buildingId));
            Strings.Add($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.DESC", description);
            Strings.Add($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.EFFECT", effect);
        }

        public static void AddPlantStrings(string plantId, string name, string description, string domesticatedDescription)
        {
            Strings.Add($"STRINGS.CREATURES.SPECIES.{plantId.ToUpperInvariant()}.NAME", UI.FormatAsLink(name, plantId));
            Strings.Add($"STRINGS.CREATURES.SPECIES.{plantId.ToUpperInvariant()}.DESC", description);
            Strings.Add($"STRINGS.CREATURES.SPECIES.{plantId.ToUpperInvariant()}.DOMESTICATEDDESC", domesticatedDescription);
        }

        public static void AddPlantSeedStrings(string plantId, string name, string description)
        {
            Strings.Add($"STRINGS.CREATURES.SPECIES.SEEDS.{plantId.ToUpperInvariant()}.NAME", UI.FormatAsLink(name, plantId));
            Strings.Add($"STRINGS.CREATURES.SPECIES.SEEDS.{plantId.ToUpperInvariant()}.DESC", description);
        }

        public static void AddFoodStrings(string foodId, string name, string description, string recipeDescription = null)
        {
            Strings.Add($"STRINGS.ITEMS.FOOD.{foodId.ToUpperInvariant()}.NAME", UI.FormatAsLink(name, foodId));
            Strings.Add($"STRINGS.ITEMS.FOOD.{foodId.ToUpperInvariant()}.DESC", description);

            if (recipeDescription != null)
                Strings.Add($"STRINGS.ITEMS.FOOD.{foodId.ToUpperInvariant()}.RECIPEDESC", recipeDescription);
        }
    }
}