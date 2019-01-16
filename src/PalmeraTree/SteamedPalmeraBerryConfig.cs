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
