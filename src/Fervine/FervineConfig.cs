using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STRINGS;
using TUNING;
using UnityEngine;
using CREATURES = STRINGS.CREATURES;

namespace Fervine
{
	public class FervineConfig : IEntityConfig
	{
		public const string ID = "Heatbulb";
		public const string SEED_ID = "HeatbulbSeed";

		public const string SeedName = "Fervine Seed";
		public static string SeedDesc = "The " + UI.FormatAsLink("Seed", "PLANTS") + " of a " + CREATURES.SPECIES.HEATBULB.NAME + ".";

		public GameObject CreatePrefab()
		{
			GameObject placedEntity = EntityTemplates.CreatePlacedEntity(ID, CREATURES.SPECIES.HEATBULB.NAME, CREATURES.SPECIES.HEATBULB.DESC, 1f,
				Assets.GetAnim("plantheatbulb_kanim"), "close", Grid.SceneLayer.BuildingFront, 1, 1, DECOR.BONUS.TIER3, defaultTemperature: 350f);
			EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 258.15f, 288.15f, 363.15f, 373.15f, pressure_sensitive: false);

			Light2D light2D = placedEntity.AddOrGet<Light2D>();
			light2D.Color = new Color(1f, 1f, 0f);
			light2D.Lux = 1800;
			light2D.Range = 5;
			placedEntity.AddOrGet<Fervine>();

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Hidden, SEED_ID,
					SeedName, SeedDesc, Assets.GetAnim("plantheatbulb_kanim"), "seedling_ground", 0, new List<Tag> { GameTags.DecorSeed },
					SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 6, "",
					EntityTemplates.CollisionShape.CIRCLE, 0.33f, 0.33f, null, string.Empty), "Heatbulb_preview", Assets.GetAnim("plantheatbulb_kanim"), "close", 1, 1);

			SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", NOISE_POLLUTION.CREATURES.TIER3);
			SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", NOISE_POLLUTION.CREATURES.TIER3);

			return placedEntity;
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
