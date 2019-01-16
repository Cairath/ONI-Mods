using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;
using CREATURES = STRINGS.CREATURES;

namespace PalmeraTree
{
	public class PalmeraTreeConfig : IEntityConfig
	{
		public const string Id = "PalmeraTreePlant";
		public const string SeedId = "PalmeraTreeSeed";

		public static string Name = UI.FormatAsLink("Palmera Tree", Id.ToUpper());
		public static string Description = $"A large, chlorine-dwelling {UI.FormatAsLink("Plant", "PLANTS")} that can be grown in farm buildings.\n\nPalmeras grow inedible buds that emit toxic hydrogen gas.";
		public static string DomesticatedDescription = $"A large, chlorine-dwelling {UI.FormatAsLink("Plant", "PLANTS")} that grows inedible buds which emit toxic hydrogen gas.";

		public static string SeedName = UI.FormatAsLink("Palmera Tree Seed", Id.ToUpper());
		public static string SeedDescription = $"The {UI.FormatAsLink("Seed", "PLANTS")} of a {Name}.";

		public GameObject CreatePrefab()
		{
			var placedEntity = EntityTemplates.CreatePlacedEntity(
				id: Id,
				name: Name,
				desc: Description,
				mass: 1f,
				anim: Assets.GetAnim("palmeratree_kanim"),
				initialAnim: "idle_loop",
				sceneLayer: Grid.SceneLayer.BuildingFront,
				width: 2,
				height: 3,
				decor: DECOR.BONUS.TIER2,
				defaultTemperature: 350f);

			EntityTemplates.ExtendEntityToBasicPlant(
				template: placedEntity,
				temperature_lethal_low: 258.15f,
				temperature_warning_low: 323.15f,
				temperature_warning_high: 363.15f,
				temperature_lethal_high: 373.15f,
				safe_elements: new[] { SimHashes.ChlorineGas },
				crop_id: PalmeraBerryConfig.Id);

			placedEntity.AddOrGet<PalmeraTree>();

			var consumer = placedEntity.AddOrGet<ElementConsumer>();
			consumer.elementToConsume = SimHashes.ChlorineGas;
			consumer.consumptionRate = 0.001f;

			var emitter = placedEntity.AddOrGet<ElementEmitter>();
			emitter.outputElement = new ElementConverter.OutputElement(0.001f, SimHashes.Hydrogen, outputElementOffsety: 2f);
			emitter.maxPressure = 1.8f;

			var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
				plant: placedEntity,
				id: SeedId,
				name: SeedName,
				desc: SeedDescription,
				productionType: SeedProducer.ProductionType.Harvest,
				anim: Assets.GetAnim("seed_palmeratree_kanim"),
				numberOfSeeds: 0,
				additionalTags: new List<Tag> { Utils.CropSeed2TileWide },
				sortOrder: 7,
				domesticatedDescription: CREATURES.SPECIES.JUNGLEGASPLANT.DOMESTICATEDDESC,
				width: 0.33f,
				height: 0.33f);

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				seed: seed,
				id: "PalmeraTree_preview",
				anim: Assets.GetAnim("palmeratree_kanim"),
				initialAnim: "clidle_wilt_loopose",
				width: 2,
				height: 3);

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
