using System.Collections.Generic;
using UnityEngine;

namespace PalmeraTree
{
	public class PalmeraBerryConfig : IEntityConfig
	{
		public static float SEEDS_PER_FRUIT_CHANCE = 0.05f;
		public static string ID = "PalmeraBerry";

		public GameObject CreatePrefab()
		{
			var entity = EntityTemplates.CreateLooseEntity(ID, "Palmera Berry", "A toxic, non-edible bud, emits hydrogen.", 1f, false,
				Assets.GetAnim((HashedString) "palmeraberry_kanim"), "object", Grid.SceneLayer.Front,
				EntityTemplates.CollisionShape.RECTANGLE, 0.77f, 0.48f, true, SimHashes.Creature, (List<Tag>)null);

			return EntityTemplates.ExtendEntityToFood(entity, new EdiblesManager.FoodInfo("PalmeraBerry", 0.0f, -1, 255.15f, 277.15f, 2400f, true));
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
