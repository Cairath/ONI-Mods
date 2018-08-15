using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace AlgaeGrower
{
	public class AlgaeGrowerConfig : IBuildingConfig
	{
		private static readonly List<Storage.StoredItemModifier> PollutedWaterStorageModifiers = new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal
		};

		public const string ID = "AlgaeGrower";

		public override BuildingDef CreateBuildingDef()
		{
			string id = ID;
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

			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, farmable, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tieR0);
			buildingDef.Floodable = false;
			buildingDef.MaterialCategory = MATERIALS.FARMABLE;
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.UtilityInputOffset = new CellOffset(0, 0);
			buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
			buildingDef.ViewMode = SimViewMode.LiquidVentMap;

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
			storage2.storageFilters = new List<Tag> { SimHashes.Algae.CreateTag(), SimHashes.CarbonDioxide.CreateTag() };

			ManualDeliveryKG manualDeliveryKg1 = go.AddOrGet<ManualDeliveryKG>();
			manualDeliveryKg1.SetStorage(storage1);
			manualDeliveryKg1.requestedItemTag = SimHashes.Fertilizer.CreateTag();
			manualDeliveryKg1.capacity = 90f;
			manualDeliveryKg1.refillMass = 18f;
			manualDeliveryKg1.choreTypeIDHash = Db.Get().ChoreTypes.OperateFetch.IdHash;

			ManualDeliveryKG manualDeliveryKg2 = go.AddComponent<ManualDeliveryKG>();
			manualDeliveryKg2.SetStorage(storage1);
			manualDeliveryKg2.requestedItemTag = SimHashes.Water.CreateTag();
			manualDeliveryKg2.capacity = 360f;
			manualDeliveryKg2.refillMass = 72f;
			manualDeliveryKg2.allowPause = true;
			manualDeliveryKg2.choreTypeIDHash = Db.Get().ChoreTypes.OperateFetch.IdHash;

			AlgaeGrower algaeHabitat = go.AddOrGet<AlgaeGrower>();
			algaeHabitat.pressureSampleOffset = new CellOffset(0, 1);

			ElementConverter elementConverter = go.AddComponent<ElementConverter>();
			elementConverter.consumedElements = new ElementConverter.ConsumedElement[3]
			{
				new ElementConverter.ConsumedElement(SimHashes.CarbonDioxide.CreateTag(), 0.01375f),
				new ElementConverter.ConsumedElement(SimHashes.Fertilizer.CreateTag(), 0.000625f),
				new ElementConverter.ConsumedElement(SimHashes.Water.CreateTag(), 0.005625f)
			};
			elementConverter.outputElements = new ElementConverter.OutputElement[2]
			{
				new ElementConverter.OutputElement(0.005f, SimHashes.Oxygen, 303.15f, false, 0.0f, 1f),
				new ElementConverter.OutputElement(0.015f, SimHashes.Algae, 303.15f, true, 0.0f, 1f)
			};

			ElementDropper elementDropper = go.AddComponent<ElementDropper>();
			elementDropper.emitMass = 5;
			elementDropper.emitTag = SimHashes.Algae.CreateTag();

			ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
			elementConsumer.elementToConsume = SimHashes.CarbonDioxide;
			elementConsumer.consumptionRate = 0.01375f;
			elementConsumer.consumptionRadius = 3;
			elementConsumer.showInStatusPanel = true;
			elementConsumer.storeOnConsume = true;
			elementConsumer.sampleCellOffset = new Vector3(0.0f, 1f, 0.0f);
			elementConsumer.isRequired = true;

			PassiveElementConsumer passiveElementConsumer = go.AddComponent<PassiveElementConsumer>();
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
