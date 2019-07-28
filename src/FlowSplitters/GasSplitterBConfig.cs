using TUNING;
using UnityEngine;

namespace FlowSplitters
{
	public class GasSplitterBConfig : IBuildingConfig
	{
		public const string Id = "GasSplitterB";
		public const string DisplayName = "Gas Splitter B";
		public const string Description = "Have you ever wanted to have your gases in two places at once?";
		public const string Effect = "Splits gases equally in two pipes. If one the output pipes can't handle half of the input, the other pipe will receive it.";
		
		private readonly ConduitPortInfo _secondaryPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(0, 1));

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 2,
				height: 2,
				anim: "gassplitter_b_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER0,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER0,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				construction_materials: MATERIALS.RAW_MINERALS,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER0,
				build_location_rule: BuildLocationRule.Conduit,
				decor: DECOR.NONE,
				noise: NOISE_POLLUTION.NONE);

			buildingDef.InputConduitType = ConduitType.Gas;
			buildingDef.OutputConduitType = ConduitType.Gas;
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.Overheatable = false;
			buildingDef.ViewMode = OverlayModes.GasConduits.ID;
			buildingDef.ObjectLayer = ObjectLayer.GasConduitConnection;
			buildingDef.SceneLayer = Grid.SceneLayer.GasConduitBridges;
			buildingDef.AudioCategory = "Metal";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.PermittedRotations = PermittedRotations.R360;
			buildingDef.UtilityInputOffset = new CellOffset(1, 0);
			buildingDef.UtilityOutputOffset = new CellOffset(0, 0);

			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, Id);

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
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
			var logic = go.AddOrGet<FlowSplitter>();
			logic.Type = ConduitType.Gas;
			logic.SecondaryPort = _secondaryPort;
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			AttachPort(go);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			Object.DestroyImmediate(go.GetComponent<RequireInputs>());
			Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
			Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());

			BuildingTemplates.DoPostConfigure(go);
		}
	}
}
