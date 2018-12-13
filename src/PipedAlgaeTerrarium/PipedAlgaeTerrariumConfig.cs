using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace PipedAlgaeTerrarium
{
	public class PipedAlgaeTerrariumConfig : IBuildingConfig
	{
		private static readonly List<Storage.StoredItemModifier> PollutedWaterStorageModifiers = new List<Storage.StoredItemModifier>()
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal
		};

		public const string ID = "AlgaeHabitatPiped";

		public override BuildingDef CreateBuildingDef()
		{
			string id = "AlgaeHabitatPiped";
			int width = 1;
			int height = 2;
			string anim = "algaefarm_kanim";
			int hitpoints = 30;
			float construction_time = 30f;
			float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
			string[] farmable = MATERIALS.FARMABLE;
			float melting_point = 1600f;
			BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
			EffectorValues tieR0 = NOISE_POLLUTION.NOISY.TIER0;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, farmable, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tieR0, 0.2f);
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
			Storage storage1 = go.AddOrGet<Storage>();
			storage1.showInUI = true;

			Storage storage2 = go.AddComponent<Storage>();
			storage2.capacityKg = 5f;
			storage2.showInUI = true;
			storage2.SetDefaultStoredItemModifiers(PollutedWaterStorageModifiers);
			storage2.allowItemRemoval = false;
			storage2.storageFilters = new List<Tag> { ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag };

			ManualDeliveryKG manualDeliveryKg1 = go.AddOrGet<ManualDeliveryKG>();
			manualDeliveryKg1.SetStorage(storage1);
			manualDeliveryKg1.requestedItemTag = new Tag("Algae");
			manualDeliveryKg1.capacity = 90f;
			manualDeliveryKg1.refillMass = 18f;
			manualDeliveryKg1.choreTypeIDHash = Db.Get().ChoreTypes.OperateFetch.IdHash;

			ManualDeliveryKG manualDeliveryKg2 = go.AddComponent<ManualDeliveryKG>();
			manualDeliveryKg2.SetStorage(storage1);
			manualDeliveryKg2.requestedItemTag = new Tag("Water");
			manualDeliveryKg2.capacity = 360f;
			manualDeliveryKg2.refillMass = 72f;
			manualDeliveryKg2.allowPause = true;
			manualDeliveryKg2.choreTypeIDHash = Db.Get().ChoreTypes.OperateFetch.IdHash;

			PipedAlgaeTerrarium algaeHabitat = go.AddOrGet<PipedAlgaeTerrarium>();
			algaeHabitat.lightBonusMultiplier = 1.1f;
			algaeHabitat.pressureSampleOffset = new CellOffset(0, 1);

			ElementConverter elementConverter = go.AddComponent<ElementConverter>();
			elementConverter.consumedElements = new ElementConverter.ConsumedElement[2]
			{
				new ElementConverter.ConsumedElement(new Tag("Algae"), 0.03f),
				new ElementConverter.ConsumedElement(new Tag("Water"), 0.3f)
			};
			elementConverter.outputElements = new ElementConverter.OutputElement[2]
			{
				new ElementConverter.OutputElement(0.04f, SimHashes.Oxygen, 303.15f, false, 0.0f, 1f, false, 1f, byte.MaxValue, 0),
				new ElementConverter.OutputElement(0.2903333f, SimHashes.DirtyWater, 303.15f, true, 0.0f, 1f, false, 1f, byte.MaxValue, 0)
			};

			ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
			conduitDispenser.conduitType = ConduitType.Liquid;
			conduitDispenser.invertElementFilter = true;
			conduitDispenser.elementFilter = new SimHashes[1]
			{
				SimHashes.Water
			};

			ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
			elementConsumer.elementToConsume = SimHashes.CarbonDioxide;
			elementConsumer.consumptionRate = 0.0003333333f;
			elementConsumer.consumptionRadius = (byte)3;
			elementConsumer.showInStatusPanel = true;
			elementConsumer.sampleCellOffset = new Vector3(0.0f, 1f, 0.0f);
			elementConsumer.isRequired = false;

			PassiveElementConsumer passiveElementConsumer = go.AddComponent<PassiveElementConsumer>();
			passiveElementConsumer.elementToConsume = SimHashes.Water;
			passiveElementConsumer.consumptionRate = 1.2f;
			passiveElementConsumer.consumptionRadius = (byte)1;
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
