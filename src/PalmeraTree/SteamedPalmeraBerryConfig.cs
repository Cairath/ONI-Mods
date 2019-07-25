using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace PalmeraTree
{
	public class SteamedPalmeraBerryConfig : IEntityConfig
	{
		public const string Id = "SteamedPalmeraBerry";
		public const string Name = "Steamed Palmera Berry";
		public static string Description = $"The steamed bud of a {PalmeraBerryConfig.NameWithLink}.\n\nLong exposure to heat and exquisite cooking skills turn the toxic berry into a delicious dessert.";
		public static string RecipeDescription = $"Delicious steamed {PalmeraBerryConfig.NameWithLink}.";
		public static LocString NameWithLink = UI.FormatAsLink(Name, Id.ToUpper());

		public ComplexRecipe Recipe;

		public GameObject CreatePrefab()
		{
			var entity = EntityTemplates.CreateLooseEntity(
				id: Id,
				name: NameWithLink,
				desc: Description,
				mass: 1f,
				unitMass: false,
				anim: Assets.GetAnim("kukumelon_kanim"),
				initialAnim: "object",
				sceneLayer: Grid.SceneLayer.Front,
				collisionShape: EntityTemplates.CollisionShape.RECTANGLE,
				width: 0.8f,
				height: 0.7f,
				isPickupable: true);

			var foodInfo = new EdiblesManager.FoodInfo(
				id: Id,
				caloriesPerUnit: 2000000f,
				quality: 6,
				preserveTemperatue: 255.15f,
				rotTemperature: 277.15f,
				spoilTime: TUNING.FOOD.SPOIL_TIME.SLOW,
				can_rot: true);

			var food = EntityTemplates.ExtendEntityToFood(entity, foodInfo);

			new Recipe(Id, 1f, 0, null, RecipeDescription, 25)
				.SetFabricator(CookingStationConfig.ID, 100f)
				.AddIngredient(new Recipe.Ingredient(PalmeraBerryConfig.Id, 1f));

			ComplexRecipe.RecipeElement[] ingredients =
			{
				new ComplexRecipe.RecipeElement(PalmeraBerryConfig.Id, 1f)
			};

			ComplexRecipe.RecipeElement[] results =
			{
				new ComplexRecipe.RecipeElement(SteamedPalmeraBerryConfig.Id, 1f)
			};

			Recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(CookingStationConfig.ID, ingredients, results), ingredients, results)
			{
				time = 100f,
				description = RecipeDescription,
                nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
                fabricators = new List<Tag> { CookingStationConfig.ID },
				sortOrder = 120
			};

			return food;
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
