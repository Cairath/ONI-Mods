using STRINGS;
using UnityEngine;
using static CaiLib.Utils.RecipeUtils;

namespace PalmeraTree
{
	public class SteamedPalmeraBerryConfig : IEntityConfig
	{
		public const string Id = "SteamedPalmeraBerry";
		public const string Name = "Steamed Palmera Berry";
		public static string Description = $"The steamed bud of a {UI.FormatAsLink(PalmeraBerryConfig.Name, PalmeraBerryConfig.Id)}.\n\nLong exposure to heat and exquisite cooking skills turn the toxic berry into a delicious dessert.";
		public static string RecipeDescription = $"Delicious steamed {UI.FormatAsLink(PalmeraBerryConfig.Name, PalmeraBerryConfig.Id)}.";

		public ComplexRecipe Recipe;

		public GameObject CreatePrefab()
		{
			var entity = EntityTemplates.CreateLooseEntity(
				id: Id,
				name: UI.FormatAsLink(Name, Id),
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
				dlcId: DlcManager.VANILLA_ID,
				caloriesPerUnit: 2000000f,
				quality: 6,
				preserveTemperatue: 255.15f,
				rotTemperature: 277.15f,
				spoilTime: TUNING.FOOD.SPOIL_TIME.SLOW,
				can_rot: true);

			var food = EntityTemplates.ExtendEntityToFood(entity, foodInfo);

			Recipe = AddComplexRecipe(
				input: new[] {new ComplexRecipe.RecipeElement(PalmeraBerryConfig.Id, 1f)},
				output: new[] {new ComplexRecipe.RecipeElement(SteamedPalmeraBerryConfig.Id, 1f)},
				fabricatorId: GourmetCookingStationConfig.ID,
				productionTime: 100f,
				recipeDescription: RecipeDescription,
				nameDisplayType: ComplexRecipe.RecipeNameDisplay.Result,
				sortOrder: 120
			);

			return food;
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}

		public string GetDlcId()
		{
			return DlcManager.VANILLA_ID;
		}
	}
}
