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
				Assets.GetAnim("plantheatbulb_kanim"), "open", Grid.SceneLayer.BuildingFront, 1, 1, DECOR.BONUS.TIER2, defaultTemperature: 350f);
			EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 258.15f, 268.15f, 363.15f, 373.15f, new[] { SimHashes.Oxygen, SimHashes.ContaminatedOxygen, SimHashes.CarbonDioxide });

			//placedEntity.AddOrGet<PalmeraTree>();

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Hidden, SEED_ID,
					SeedName, SeedDesc, Assets.GetAnim("plantheatbulb_kanim"), "misc", 0, new List<Tag> { GameTags.DecorSeed },
					SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 6, "",
					EntityTemplates.CollisionShape.CIRCLE, 0.33f, 0.33f, null, string.Empty), "PalmeraTree_preview", Assets.GetAnim("palmeratree_kanim"), "idle_wilt_loop", 1, 1);

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
