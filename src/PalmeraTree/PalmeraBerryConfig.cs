using STRINGS;
using UnityEngine;

namespace PalmeraTree
{
	public class PalmeraBerryConfig : IEntityConfig
	{
		public static string ID = "PalmeraBerry";
		public static string NameStr = "Palmera Berry";

		public static LocString Name = (LocString)UI.FormatAsLink(NameStr, ID.ToUpper());
		public static LocString Desc = (LocString)"A toxic, non-edible bud that emits hydrogen.";

		public GameObject CreatePrefab()
		{
			var entity = EntityTemplates.CreateLooseEntity(ID, Name, Desc, 1f, false,
				Assets.GetAnim("palmeraberry_kanim"), "object", Grid.SceneLayer.Front,
				EntityTemplates.CollisionShape.RECTANGLE, 0.77f, 0.48f, true);

			var foodEntity = EntityTemplates.ExtendEntityToFood(entity, new EdiblesManager.FoodInfo(ID, 0.0f, TUNING.FOOD.FOOD_QUALITY_AWFUL, 255.15f, 277.15f, TUNING.FOOD.SPOIL_TIME.SLOW, true));

			Sublimates sublimates = foodEntity.AddOrGet<Sublimates>();
			sublimates.spawnFXHash = SpawnFXHashes.OxygenEmissionBubbles;
			sublimates.info = new Sublimates.Info(0.001f, 0f, 1.8f, 0.0f, SimHashes.Hydrogen);

			return foodEntity;
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}