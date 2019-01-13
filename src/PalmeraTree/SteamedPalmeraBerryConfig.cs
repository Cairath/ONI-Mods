using STRINGS;
using TUNING;
using UnityEngine;

namespace PalmeraTree
{
	public class SteamedPalmeraBerryConfig : IEntityConfig
	{
		public const string ID = "SteamedPalmeraBerry";
		public const string NameStr = "Steamed Palmera Berry";

		public static LocString Name = (LocString)UI.FormatAsLink(NameStr, ID.ToUpper());
		public static LocString Desc = (LocString)("The steamed bud of a " + PalmeraBerryConfig.NameStr + ".\n\nLong exposure to heat and exquisite cooking skills turn the toxic berry into a delicious dessert.");
		public static LocString RecipeDesc = (LocString)("Delicious steamed " + PalmeraBerryConfig.NameStr + ".");

		public GameObject CreatePrefab()
		{
			GameObject food = EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(ID, Name, Desc, 1f, false, Assets.GetAnim("kukumelon_kanim"), "object",
				Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.7f, true), new EdiblesManager.FoodInfo(ID, 2000000f, 6, 255.15f, 277.15f, FOOD.SPOIL_TIME.SLOW, true));
			new Recipe(ID, 1f, 0, null, RecipeDesc, 25).SetFabricator(CookingStationConfig.ID, 100f).AddIngredient(new Recipe.Ingredient(PalmeraBerryConfig.ID, 1f));
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