﻿using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;
using CREATURES = STRINGS.CREATURES;

namespace RanchingRebalanced.PalmeraTree
{
	public class PalmeraTreeConfig : IEntityConfig
	{
		public const string ID = "PalmeraTreePlant";
		public const string SEED_ID = "PalmeraTreeSeed";

		public GameObject CreatePrefab()
		{
			GameObject placedEntity = EntityTemplates.CreatePlacedEntity(ID, CREATURES.SPECIES.JUNGLEGASPLANT.NAME,CREATURES.SPECIES.JUNGLEGASPLANT.DESC, 1f,
				Assets.GetAnim("palmeratree_kanim"), "idle_loop", Grid.SceneLayer.BuildingFront, 1, 3, DECOR.BONUS.TIER2);
			EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 258.15f, 278.15f, 308.15f, 318.15f, new SimHashes[1] { SimHashes.ChlorineGas }, true, 0.0f, 0.15f, PalmeraBerryConfig.ID);

			placedEntity.AddOrGet<PalmeraTree>();
			var consumer = placedEntity.AddOrGet<ElementConsumer>();
			consumer.elementToConsume = SimHashes.ChlorineGas;
			consumer.consumptionRate = 0.001f;
				
			var emitter = placedEntity.AddOrGet<ElementEmitter>();
			emitter.outputElement = new ElementConverter.OutputElement(0.001f, SimHashes.Hydrogen, outputElementOffsety: 2f);
			emitter.maxPressure = 1.8f;

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Harvest, SEED_ID,
					"Palmera Tree Seed", "The " + UI.FormatAsLink("Seed", "PLANTS") + " of a " + CREATURES.SPECIES.JUNGLEGASPLANT.NAME + ".",
					Assets.GetAnim("seed_palmeratree_kanim"), "object", 0, new List<Tag> { GameTags.CropSeed },
					SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 6, CREATURES.SPECIES.JUNGLEGASPLANT.DOMESTICATEDDESC,
					EntityTemplates.CollisionShape.CIRCLE, 0.33f, 0.33f, null, string.Empty), "PalmeraTree_preview", Assets.GetAnim("palmeratree_kanim"), "idle_wilt_loop", 1, 3);

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
