using STRINGS;
using TUNING;
using UnityEngine;
using BUILDINGS = TUNING.BUILDINGS;

namespace ConveyorShutoff
{
    public class SolidConduitShutoffConfig : IBuildingConfig
	{
		private static readonly LogicPorts.Port[] INPUT_PORTS = new LogicPorts.Port[1]
		{
			LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), UI.LOGIC_PORTS.CONTROL_OPERATIONAL, true)
		};

		public const string ID = "SolidConduitShutoff";

		public override BuildingDef CreateBuildingDef()
		{
			string id = ID;
			int width = 3;
			int height = 1;
			string anim = "utilities_conveyorbridge_kanim";
			int hitpoints = 100;
			float construction_time = 60f;
			float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
			string[] refinedMetals = MATERIALS.REFINED_METALS;
			float melting_point = 1600f;

			BuildLocationRule build_location_rule = BuildLocationRule.Conduit;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, refinedMetals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
			buildingDef.InputConduitType = ConduitType.Solid;
			buildingDef.OutputConduitType = ConduitType.Solid;
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.ViewMode = SimViewMode.SolidConveyorMap;
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
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, id);
			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddPrefabTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);

			GeneratedBuildings.RegisterLogicPorts(go, SolidConduitShutoffConfig.INPUT_PORTS);

			Constructable component = go.GetComponent<Constructable>();
			component.choreTags = GameTags.ChoreTypes.ConveyorChores;
			component.requiredRolePerk = RoleManager.rolePerks.ConveyorBuild.id;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, SolidConduitShutoffConfig.INPUT_PORTS);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, SolidConduitShutoffConfig.INPUT_PORTS);

			go.GetComponent<KPrefabID>().AddPrefabTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			go.AddOrGet<SolidConduitShutoff>();
			go.GetComponent<RequireInputs>().SetRequirements(true, false);
			go.AddOrGet<LogicOperationalController>();
			go.AddOrGet<LogicOperationalController>().unNetworkedValue = 0;
			BuildingTemplates.DoPostConfigure(go);
		}
	}
}
