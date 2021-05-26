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

        public static string Name = "Palmera Tree";
        public static string Description = $"A large, chlorine-dwelling {UI.FormatAsLink("Plant", "PLANTS")} that can be grown in farm buildings.\n\nPalmeras grow inedible buds that emit toxic hydrogen gas.";
        public static string DomesticatedDescription = $"A large, chlorine-dwelling {UI.FormatAsLink("Plant", "PLANTS")} that grows inedible buds which emit toxic hydrogen gas.";

        public static string SeedName = "Palmera Tree Seed";
        public static string SeedDescription = $"The {UI.FormatAsLink("Seed", "PLANTS")} of a {UI.FormatAsLink(Name, Id)}.";

        private const string AnimName = "custom_palmeratree_kanim";
        private const string AnimNameSeed = "seed_palmeratree_kanim";

        public GameObject CreatePrefab()
        {
            var placedEntity = EntityTemplates.CreatePlacedEntity(
                id: Id,
                name: Name,
                desc: Description,
                mass: 1f,
                anim: Assets.GetAnim(AnimName),
                initialAnim: "idle_loop",
                sceneLayer: Grid.SceneLayer.BuildingFront,
                width: 1,
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
                crop_id: PalmeraBerryConfig.Id,
                baseTraitId: $"{Id}Original",
                baseTraitName: Name);

            placedEntity.AddOrGet<PalmeraTree>();

            var consumer = placedEntity.AddOrGet<ElementConsumer>();
            consumer.elementToConsume = SimHashes.ChlorineGas;
            consumer.consumptionRate = 0.001f;

            var emitter = placedEntity.AddOrGet<ElementEmitter>();
            emitter.outputElement = new ElementConverter.OutputElement(0.001f, SimHashes.Hydrogen, 0f, true, false, 0f, 2f);
            emitter.maxPressure = 1.8f;

            var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
                plant: placedEntity,
                id: SeedId,
                name: UI.FormatAsLink(SeedName, Id),
                desc: SeedDescription,
                productionType: SeedProducer.ProductionType.Harvest,
                anim: Assets.GetAnim(AnimNameSeed),
                numberOfSeeds: 0,
                additionalTags: new List<Tag> { GameTags.CropSeed },
                sortOrder: 7,
                domesticatedDescription: CREATURES.SPECIES.JUNGLEGASPLANT.DOMESTICATEDDESC,
                width: 0.33f,
                height: 0.33f);

            EntityTemplates.CreateAndRegisterPreviewForPlant(
                seed: seed,
                id: "PalmeraTree_preview",
                anim: Assets.GetAnim(AnimName),
                initialAnim: "place",
                width: 2,
                height: 3);

            SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", NOISE_POLLUTION.CREATURES.TIER1);

            return placedEntity;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }

        public string GetDlcId()
        {
	        return DlcManager.VANILLA_ID;
        }
    }
}
