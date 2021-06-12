using STRINGS;
using UnityEngine;

namespace PalmeraTree
{
	public class PalmeraBerryConfig : IEntityConfig
	{
		public static string Id = "PalmeraBerry";
		public static string Name = "Palmera Berry";
		public static string Description = "A toxic, non-edible bud that emits hydrogen.";

		public GameObject CreatePrefab()
		{
			var entity = EntityTemplates.CreateLooseEntity(
				id: Id,
				name: UI.FormatAsLink(Name, Id),
				desc: Description,
				mass: 1f,
				unitMass: false,
				anim: Assets.GetAnim("palmeraberry_kanim"),
				initialAnim: "object",
				sceneLayer: Grid.SceneLayer.Front,
				collisionShape: EntityTemplates.CollisionShape.RECTANGLE,
				width: 0.77f,
				height: 0.48f,
				isPickupable: true);

			var foodInfo = new EdiblesManager.FoodInfo(
				id: Id,
				dlcId: DlcManager.VANILLA_ID,
				caloriesPerUnit: 0.0f,
				quality: TUNING.FOOD.FOOD_QUALITY_AWFUL,
				preserveTemperatue: 255.15f,
				rotTemperature: 277.15f,
				spoilTime: TUNING.FOOD.SPOIL_TIME.SLOW,
				can_rot: true);

			var foodEntity = EntityTemplates.ExtendEntityToFood(entity, foodInfo);

			var sublimates = foodEntity.AddOrGet<Sublimates>();
			sublimates.spawnFXHash = SpawnFXHashes.OxygenEmissionBubbles;
			sublimates.info = new Sublimates.Info(
				rate: 0.001f,
				min_amount: 0f,
				max_destination_mass: 1.8f,
				mass_power: 0.0f,
				element: SimHashes.Hydrogen);

			return foodEntity;
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}

		public string[] GetDlcIds()
		{
			return DlcManager.AVAILABLE_ALL_VERSIONS;
		}
	}
}
