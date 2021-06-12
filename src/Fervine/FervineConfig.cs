using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

namespace Fervine
{
	public class FervineConfig : IEntityConfig
	{
		public const string Id = "Fervine";
		public const string Name = "Fervine";
		public static string Description = $"A temperature reactive, subterranean {UI.FormatAsLink("Plant", "PLANTS")}.";
		public static string DomesticatedDescription = $"Fervine uses tiny amounts of heat energy from the atmosphere to keep its light up. " +
		                                               $"It won't use any if the temperature falls under {GameUtil.GetFormattedTemperature(293.15f)}.";

		public const string SeedId = "FervineBulb";
		public const string SeedName = "Fervine Bulb";
		public static string SeedDesc = $"The {UI.FormatAsLink("Seed", "PLANTS")} of a {UI.FormatAsLink(Name, Id)}.";

		private const string AnimName = "fervine_kanim";
		private const string AnimNameSeed = "seed_fervine_kanim";

		public GameObject CreatePrefab()
		{
			var plantEntityTemplate = EntityTemplates.CreatePlacedEntity(
				id: Id,
				name: UI.FormatAsLink(Name, Id),
				desc: Description,
				width: 1,
				height: 1,
				mass: 1f,
				anim: Assets.GetAnim(AnimName),
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
				can_tinker: false,
				baseTraitId: $"{Id}Original",
				baseTraitName: Name);

			var light2D = plantEntityTemplate.AddOrGet<Light2D>();
			light2D.Color = new Color(1f, 1f, 0f);
			light2D.Lux = 1800;
			light2D.Range = 5;

			plantEntityTemplate.AddOrGet<Fervine>();

			var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
				plant: plantEntityTemplate,
				id: SeedId,
				name: UI.FormatAsLink(SeedName, Id),
				desc: SeedDesc,
				productionType: SeedProducer.ProductionType.Hidden,
				anim: Assets.GetAnim(AnimNameSeed),
				numberOfSeeds: 0,
				additionalTags: new List<Tag> { GameTags.DecorSeed },
				sortOrder: 6,
				width: 0.33f,
				height: 0.33f);

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				seed: seed,
				id: "Heatbulb_preview",
				anim: Assets.GetAnim(AnimName),
				initialAnim: "place",
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

		public string[] GetDlcIds()
		{
			return DlcManager.AVAILABLE_ALL_VERSIONS;
		}
	}
}
