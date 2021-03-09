using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;
using BUILDINGS = TUNING.BUILDINGS;

namespace AlgaeGrower
{
	public class AlgaeGrowerConfig : IBuildingConfig
	{
		public const string Id = "AlgaeGrower";
		public const string DisplayName = "Algae Grower";
		public const string Description = "Algae colony, Duplicant colony... we're more alike than we are different.";
		public static string Effect =
			$"Consumes {GameTags.Agriculture.Name}, {ELEMENTS.CARBONDIOXIDE.NAME} and {ELEMENTS.WATER.NAME} " +
			$"to grow {ELEMENTS.ALGAE.NAME} and emit {ELEMENTS.OXYGEN.NAME}.\n\nRequires {UI.FormatAsLink("Light", "LIGHT")}  to grow.";
		
		private const float MATERIAL_RATE = 0.01125f;
		private const float OXYGEN_RATE = 0.04f;
		private const float CO2_RATE = 0.01375f;
		private const float BASE_CAPACITY = 9f;
		private const float BASE_REFILL_MASS = 3f;
		private const float BASE_TEMPERATURE = 303.15f;

		private static readonly List<Storage.StoredItemModifier> hiddenStorageModifiers = new List<Storage.StoredItemModifier>{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal
		};

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 1,
				height: 2,
				anim: "algaegrower_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER1,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
				construction_materials: MATERIALS.FARMABLE,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				build_location_rule: BuildLocationRule.OnFloor,
				decor: BUILDINGS.DECOR.PENALTY.TIER1,
				noise: NOISE_POLLUTION.NOISY.TIER0);

			buildingDef.Floodable = false;
			buildingDef.MaterialCategory = MATERIALS.FARMABLE;
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.UtilityInputOffset = new CellOffset(0, 0);
			buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
			buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;

			SoundEventVolumeCache.instance.AddVolume("algaefarm_kanim", "AlgaeHabitat_bubbles", NOISE_POLLUTION.NOISY.TIER0);
			SoundEventVolumeCache.instance.AddVolume("algaefarm_kanim", "AlgaeHabitat_algae_in", NOISE_POLLUTION.NOISY.TIER0);
			SoundEventVolumeCache.instance.AddVolume("algaefarm_kanim", "AlgaeHabitat_algae_out", NOISE_POLLUTION.NOISY.TIER0);

			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			configureStorage(go);

			var algaeHabitat = go.AddOrGet<AlgaeGrower>();
			algaeHabitat.PressureSampleOffset = new CellOffset(0, 1);

			configureItemConversions(go);

			go.AddOrGet<AnimTileable>();

			Prioritizable.AddRef(go);
		}

		private void configureStorage(GameObject go)
        {
			var storageBase = go.AddOrGet<Storage>();
			storageBase.showInUI = true;

			//Algae
			var storageAlgea = go.AddComponent<Storage>();
			storageAlgea.capacityKg = BASE_CAPACITY;
			storageAlgea.showInUI = true;
			storageAlgea.SetDefaultStoredItemModifiers(hiddenStorageModifiers);
			storageAlgea.allowItemRemoval = false;
			storageAlgea.storageFilters = new List<Tag> { GameTags.Algae };

			//Agriculture
			var manualDeliveryFertilizer = go.AddOrGet<ManualDeliveryKG>();
			manualDeliveryFertilizer.SetStorage(storageBase);
			manualDeliveryFertilizer.requestedItemTag = GameTags.Agriculture;
			manualDeliveryFertilizer.capacity = BASE_CAPACITY;
			manualDeliveryFertilizer.refillMass = BASE_REFILL_MASS;
			manualDeliveryFertilizer.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;

			//Water
			var manualDeliveryWater = go.AddComponent<ManualDeliveryKG>();
			manualDeliveryWater.SetStorage(storageBase);
			manualDeliveryWater.requestedItemTag = GameTags.Water;
			manualDeliveryWater.capacity = BASE_CAPACITY;
			manualDeliveryWater.refillMass = BASE_REFILL_MASS;
			manualDeliveryWater.allowPause = true;
			manualDeliveryWater.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		}

		private void configureItemConversions(GameObject go)
        {
			var elementConverter = go.AddComponent<ElementConverter>();
			elementConverter.consumedElements = new[]
			{
				new ElementConverter.ConsumedElement(GameTags.Agriculture, MATERIAL_RATE),
				new ElementConverter.ConsumedElement(GameTags.Water, MATERIAL_RATE)
			};
			elementConverter.outputElements = new[]
			{
				new ElementConverter.OutputElement(OXYGEN_RATE, SimHashes.Oxygen, BASE_TEMPERATURE, storeOutput: false),
				new ElementConverter.OutputElement(MATERIAL_RATE*2, SimHashes.Algae, BASE_TEMPERATURE, storeOutput: true)
			};

			//Drop algea
			var elementDropperAlgae = go.AddComponent<ElementDropper>();
			elementDropperAlgae.emitMass = BASE_CAPACITY;
			elementDropperAlgae.emitTag = GameTags.Algae;

			//Consume Co2
			var elementConsumerCarbonDioxide = go.AddOrGet<ElementConsumer>();
			elementConsumerCarbonDioxide.elementToConsume = SimHashes.CarbonDioxide;
			elementConsumerCarbonDioxide.consumptionRate = CO2_RATE;
			elementConsumerCarbonDioxide.consumptionRadius = 3;
			elementConsumerCarbonDioxide.showInStatusPanel = true;
			elementConsumerCarbonDioxide.storeOnConsume = false;
			elementConsumerCarbonDioxide.sampleCellOffset = new Vector3(0.0f, 1f, 0.0f);
			elementConsumerCarbonDioxide.isRequired = false;

			//Store water from environment
			var passiveElementConsumerWater = go.AddComponent<PassiveElementConsumer>();
			passiveElementConsumerWater.elementToConsume = SimHashes.Water;
			passiveElementConsumerWater.consumptionRate = 1.5f;
			passiveElementConsumerWater.consumptionRadius = 1;
			passiveElementConsumerWater.showDescriptor = false;
			passiveElementConsumerWater.storeOnConsume = true;
			passiveElementConsumerWater.capacityKG = BASE_CAPACITY;
			passiveElementConsumerWater.showInStatusPanel = false;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{

		}
	}
}
