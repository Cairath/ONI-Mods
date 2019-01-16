using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;
using CREATURES = STRINGS.CREATURES;

namespace Fervine
{
	public class FervineConfig : IEntityConfig
	{
		public const string Id = "Heatbulb";
		public const string SeedId = "HeatbulbSeed";

		public const string SeedName = "Fervine Seed";
		public static string SeedDesc = $"The {UI.FormatAsLink("Seed", "PLANTS")} of a {CREATURES.SPECIES.HEATBULB.NAME}.";

		public GameObject CreatePrefab()
		{
			var plantEntityTemplate = EntityTemplates.CreatePlacedEntity(
				id: Id,
				name: CREATURES.SPECIES.HEATBULB.NAME,
				desc: CREATURES.SPECIES.HEATBULB.DESC,
				width: 1,
				height: 1,
				mass: 1f,
				anim: Assets.GetAnim("plantheatbulb_kanim"),
				initialAnim: "close",
				sceneLayer: Grid.SceneLayer.BuildingFront,
				decor: DECOR.BONUS.TIER3,
				defaultTemperature: 350f);

			EntityTemplates.ExtendEntityToBasicPlant(
				template: plantEntityTemplate,
				temperature_lethal_low: 258.15f,
				temperature_warning_low: 288.15f,
				temperature_warning_high: 363.15f,
				temperature_lethal_high: 373.15f,
				pressure_sensitive: false,
				can_tinker: false);

			var light2D = plantEntityTemplate.AddOrGet<Light2D>();
			light2D.Color = new Color(1f, 1f, 0f);
			light2D.Lux = 1800;
			light2D.Range = 5;

			plantEntityTemplate.AddOrGet<Fervine>();

			var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
				plant: plantEntityTemplate,
				id: SeedId,
				name: SeedName,
				desc: SeedDesc,
				productionType: SeedProducer.ProductionType.Hidden,
				anim: Assets.GetAnim("plantheatbulb_kanim"),
				initialAnim: "seed_swamplily",
				numberOfSeeds: 0,
				additionalTags: new List<Tag> { GameTags.DecorSeed },
				sortOrder: 6,
				width: 0.33f,
				height: 0.33f);

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				seed: seed,
				id: "Heatbulb_preview",
				anim: Assets.GetAnim("plantheatbulb_kanim"),
				initialAnim: "close",
				width: 1,
				height: 1);

			SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", NOISE_POLLUTION.CREATURES.TIER3);

			return plantEntityTemplate;
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
