using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;
using BUILDINGS = TUNING.BUILDINGS;

namespace PipedAlgaeTerrarium
{
	public class PipedAlgaeTerrariumConfig : IBuildingConfig
	{
		public const string Id = "AlgaeHabitatPiped";
		public const string DisplayName = "Piped Algae Terrarium";
		public const string Description = "Algae colony, Duplicant colony... we're more alike than we are different.";
		public static string Effect = $"Consumes {ELEMENTS.ALGAE.NAME} to produce {ELEMENTS.OXYGEN.NAME} and remove some {ELEMENTS.CARBONDIOXIDE.NAME}.\n\n" +
									  $"Gains a 10% efficiency boost in direct {UI.FormatAsLink("Light", "LIGHT")}.";

		private static readonly List<Storage.StoredItemModifier> PollutedWaterStorageModifiers = new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal
		};

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 1,
				height: 2,
				anim: "algaefarm_kanim",
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
			buildingDef.OutputConduitType = ConduitType.Liquid;
			buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;

			SoundEventVolumeCache.instance.AddVolume("algaefarm_kanim", "AlgaeHabitat_bubbles", NOISE_POLLUTION.NOISY.TIER0);
			SoundEventVolumeCache.instance.AddVolume("algaefarm_kanim", "AlgaeHabitat_algae_in", NOISE_POLLUTION.NOISY.TIER0);
			SoundEventVolumeCache.instance.AddVolume("algaefarm_kanim", "AlgaeHabitat_algae_out", NOISE_POLLUTION.NOISY.TIER0);

			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			var storage1 = go.AddOrGet<Storage>();
			storage1.showInUI = true;

			var storage2 = go.AddComponent<Storage>();
			storage2.capacityKg = 5f;
			storage2.showInUI = true;
			storage2.SetDefaultStoredItemModifiers(PollutedWaterStorageModifiers);
			storage2.allowItemRemoval = false;
			storage2.storageFilters = new List<Tag> { ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag };

			var manualDeliveryKg1 = go.AddOrGet<ManualDeliveryKG>();
			manualDeliveryKg1.SetStorage(storage1);
			manualDeliveryKg1.requestedItemTag = SimHashes.Algae.CreateTag();
			manualDeliveryKg1.capacity = 90f;
			manualDeliveryKg1.refillMass = 18f;
			manualDeliveryKg1.choreTypeIDHash = Db.Get().ChoreTypes.OperateFetch.IdHash;

			var manualDeliveryKg2 = go.AddComponent<ManualDeliveryKG>();
			manualDeliveryKg2.SetStorage(storage1);
			manualDeliveryKg2.requestedItemTag = SimHashes.Water.CreateTag();
			manualDeliveryKg2.capacity = 360f;
			manualDeliveryKg2.refillMass = 72f;
			manualDeliveryKg2.allowPause = true;
			manualDeliveryKg2.choreTypeIDHash = Db.Get().ChoreTypes.OperateFetch.IdHash;

			var algaeHabitat = go.AddOrGet<PipedAlgaeTerrarium>();
			algaeHabitat.LightBonusMultiplier = 1.1f;
			algaeHabitat.PressureSampleOffset = new CellOffset(0, 1);

			var elementConverter = go.AddComponent<ElementConverter>();
			elementConverter.consumedElements = new[]
			{
				new ElementConverter.ConsumedElement(SimHashes.Algae.CreateTag(), 0.03f),
				new ElementConverter.ConsumedElement(SimHashes.Water.CreateTag(), 0.3f)
			};

			elementConverter.outputElements = new[]
			{
				new ElementConverter.OutputElement(0.04f, SimHashes.Oxygen, 303.15f, false, 0.0f, 1f, false, 1f, byte.MaxValue, 0),
				new ElementConverter.OutputElement(0.2903333f, SimHashes.DirtyWater, 303.15f, true, 0.0f, 1f, false, 1f, byte.MaxValue, 0)
			};

			var conduitDispenser = go.AddOrGet<ConduitDispenser>();
			conduitDispenser.conduitType = ConduitType.Liquid;
			conduitDispenser.invertElementFilter = true;
			conduitDispenser.elementFilter = new[] { SimHashes.Water };

			var elementConsumer = go.AddOrGet<ElementConsumer>();
			elementConsumer.elementToConsume = SimHashes.CarbonDioxide;
			elementConsumer.consumptionRate = 0.0003333333f;
			elementConsumer.consumptionRadius = 3;
			elementConsumer.showInStatusPanel = true;
			elementConsumer.sampleCellOffset = new Vector3(0.0f, 1f, 0.0f);
			elementConsumer.isRequired = false;

			var passiveElementConsumer = go.AddComponent<PassiveElementConsumer>();
			passiveElementConsumer.elementToConsume = SimHashes.Water;
			passiveElementConsumer.consumptionRate = 1.2f;
			passiveElementConsumer.consumptionRadius = 1;
			passiveElementConsumer.showDescriptor = false;
			passiveElementConsumer.storeOnConsume = true;
			passiveElementConsumer.capacityKG = 360f;
			passiveElementConsumer.showInStatusPanel = false;

			go.AddOrGet<AnimTileable>();

			Prioritizable.AddRef(go);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			BuildingTemplates.DoPostConfigure(go);
		}
	}
}
