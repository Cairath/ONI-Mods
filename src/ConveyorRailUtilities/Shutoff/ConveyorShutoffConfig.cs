using STRINGS;
using TUNING;
using UnityEngine;
using BUILDINGS = TUNING.BUILDINGS;

namespace ConveyorRailUtilities.Shutoff
{
	public class ConveyorShutoffConfig : IBuildingConfig
	{
		public const string Id = "ConveyorShutoff";
		public const string DisplayName = "Conveyor Shutoff";
		public const string Description = "Your items won't go anywhere unless you let them.";
		public const string Effect = "Automatically turns flow of objects on the Conveyor Rail on or off using Automation technology.";

		private static readonly LogicPorts.Port[] InputPorts = {
			LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), UI.LOGIC_PORTS.CONTROL_OPERATIONAL, true)
		};

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
			buildingDef.EnergyConsumptionWhenActive = 10f;
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

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);

			GeneratedBuildings.RegisterLogicPorts(go, InputPorts);

			var component = go.GetComponent<Constructable>();
			component.choreTags = GameTags.ChoreTypes.ConveyorChores;
			component.requiredRolePerk = RoleManager.rolePerks.ConveyorBuild.id;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, InputPorts);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, InputPorts);

			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			go.AddOrGet<ConveyorShutoff>();
			go.GetComponent<RequireInputs>().SetRequirements(true, false);
			go.AddOrGet<LogicOperationalController>();
			go.AddOrGet<LogicOperationalController>().unNetworkedValue = 0;
			BuildingTemplates.DoPostConfigure(go);
		}
	}
}
