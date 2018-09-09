using UnityEngine;

namespace PalmeraTree
{
	public class PalmeraBerryConfig : IEntityConfig
	{
		public static string ID = "PalmeraBerry";

		public GameObject CreatePrefab()
		{
			var entity = EntityTemplates.CreateLooseEntity(ID, "Palmera Berry", "A toxic, non-edible bud that emits hydrogen.", 1f, false,
				Assets.GetAnim("palmeraberry_kanim"), "object", Grid.SceneLayer.Front,
				EntityTemplates.CollisionShape.RECTANGLE, 0.77f, 0.48f, true);

			var foodEntity = EntityTemplates.ExtendEntityToFood(entity, new EdiblesManager.FoodInfo(ID, 0.0f, -1, 255.15f, 277.15f, 2400f, true));

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