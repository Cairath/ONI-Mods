using TUNING;
using UnityEngine;

namespace FlowSplitters
{
	public class LiquidSplitterConfig : IBuildingConfig
	{
		public const string Id = "LiquidSplitter";
		public const string DisplayName = "Liquid Splitter";
		public const string Description = "Splits liquids equally in two pipes. If one the output pipes can't handle half of the input, the other pipe will receive it.";
		public const string Effect = "Have you ever wanted to have your liquids in two places at once?";

		private readonly ConduitPortInfo _secondaryPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(1, 1));

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 2,
				height: 2,
				anim: "utilityliquidsplitter_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER0,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER0,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				construction_materials: MATERIALS.RAW_MINERALS,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER0,
				build_location_rule: BuildLocationRule.Conduit,
				decor: DECOR.NONE,
				noise: NOISE_POLLUTION.NONE);

			buildingDef.InputConduitType = ConduitType.Liquid;
			buildingDef.OutputConduitType = ConduitType.Liquid;
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.Overheatable = false;
			buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
			buildingDef.ObjectLayer = ObjectLayer.LiquidConduitConnection;
			buildingDef.SceneLayer = Grid.SceneLayer.LiquidConduitBridges;
			buildingDef.AudioCategory = "Metal";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.PermittedRotations = PermittedRotations.R360;
			buildingDef.UtilityInputOffset = new CellOffset(0, 0);
			buildingDef.UtilityOutputOffset = new CellOffset(1, 0);

			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, Id);

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
			logic.Type = ConduitType.Liquid;
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
