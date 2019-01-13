using TUNING;
using UnityEngine;

namespace ConveyorRailUtilities.Dropoff
{
	public class ConveyorDropoffConfig : IBuildingConfig
	{
		public const string Id = "ConveyorDropoff";
		public const string DisplayName = "Conveyor Dropoff Point";
		public const string Description = "A garbage collector!";
		public const string Effect = "A place for the Auto-Sweepers to drop stuff on the ground.";

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 1,
				height: 3,
				anim: "relocator_dropoff_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER1,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				construction_materials: MATERIALS.ALL_METALS,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				build_location_rule: BuildLocationRule.OnFloor,
				decor: BUILDINGS.DECOR.PENALTY.TIER1,
				noise: NOISE_POLLUTION.NONE);

			buildingDef.Floodable = false;
			buildingDef.Overheatable = false;
			buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.InputConduitType = ConduitType.Solid;
			buildingDef.UtilityInputOffset = new CellOffset(0, 1);
			buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidConduitOutbox");

			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			go.AddOrGet<ConveyorDropoff>();
			go.AddOrGet<SolidConduitConsumer>();

			var defaultStorage = BuildingTemplates.CreateDefaultStorage(go);
			defaultStorage.capacityKg = 100f;
			defaultStorage.showInUI = false;
			defaultStorage.allowItemRemoval = false;

			go.AddOrGet<SimpleVent>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			Prioritizable.AddRef(go);
			go.AddOrGet<Automatable>();
		}
	}
}
