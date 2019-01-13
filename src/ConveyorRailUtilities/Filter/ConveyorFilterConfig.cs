using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace ConveyorRailUtilities.Filter
{
	public class ConveyorFilterConfig : IBuildingConfig
	{
		public const string Id = "ConveyorFilter";
		public const string DisplayName = "Conveyor Rail Filter";
		public const string Description = "Filters incoming items on the Conveyor Rail. Filtered items (selected on the list) are put on the secondary output in the middle of the filter (icon not visible).";
		public const string Effect = "Filters the Conveyor Rail by sending selected items to a separate output.";

		private readonly ConduitPortInfo _secondaryPort = new ConduitPortInfo(ConduitType.Solid, new CellOffset(0, 0));

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 3,
				height: 1,
				anim: "utilities_conveyorbridge_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER2,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
				construction_materials: MATERIALS.REFINED_METALS,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				build_location_rule: BuildLocationRule.Conduit,
				decor: BUILDINGS.DECOR.NONE,
				noise: NOISE_POLLUTION.NONE);

			buildingDef.InputConduitType = ConduitType.Solid;
			buildingDef.OutputConduitType = ConduitType.Solid;
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 120f;
			buildingDef.ExhaustKilowattsWhenActive = 0.0f;
			buildingDef.SelfHeatKilowattsWhenActive = 2f;
			buildingDef.AudioCategory = "Metal";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.PermittedRotations = PermittedRotations.R360;
			buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
			buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
			buildingDef.PowerInputOffset = new CellOffset(0, 0);

			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, Id);

			return buildingDef;
		}

		private void AttachPort(GameObject go)
		{
			go.AddComponent<ConduitSecondaryOutput>().portInfo = _secondaryPort;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			base.DoPostConfigurePreview(def, go);
			AttachPort(go);
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			AttachPort(go);

			var component = go.GetComponent<Constructable>();
			component.choreTags = GameTags.ChoreTypes.ConveyorChores;
			component.requiredRolePerk = RoleManager.rolePerks.ConveyorBuild.id;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			BuildingTemplates.DoPostConfigure(go);
			Prioritizable.AddRef(go);

			go.AddOrGet<EnergyConsumer>();
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

			var tagList = new List<Tag>();
			tagList.AddRange(STORAGEFILTERS.NOT_EDIBLE_SOLIDS);
			tagList.AddRange(STORAGEFILTERS.FOOD);

			var storage = go.AddOrGet<Storage>();
			storage.capacityKg = 0f;
			storage.showInUI = true;
			storage.showDescriptor = true;
			storage.storageFilters = tagList;
			storage.allowItemRemoval = false;
			storage.onlyTransferFromLowerPriority = false;

			go.AddOrGet<Automatable>();
			go.AddOrGet<TreeFilterable>();

			var filterLogic = go.AddOrGet<ConveyorFilter>();
			filterLogic.SecondaryPort = _secondaryPort;

			go.AddOrGetDef<PoweredActiveController.Def>().showWorkingStatus = true;
		}
	}
}
