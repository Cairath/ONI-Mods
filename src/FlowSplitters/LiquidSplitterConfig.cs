using TUNING;
using UnityEngine;

namespace FlowSplitters
{
	public class LiquidSplitterConfig : IBuildingConfig
	{
		public const string ID = "LiquidSplitter";
		private ConduitPortInfo secondaryPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(1, 1));

		public override BuildingDef CreateBuildingDef()
		{
			string id = ID;
			int width = 2;
			int height = 2;
			string anim = "utilityliquidsplitter_kanim";
			int hitpoints = 10;
			float construction_time = 3f;
			float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
			string[] refinedMetals = MATERIALS.RAW_MINERALS;
			float melting_point = 1600f;

			BuildLocationRule build_location_rule = BuildLocationRule.Conduit;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, refinedMetals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
			buildingDef.InputConduitType = ConduitType.Liquid;
			buildingDef.OutputConduitType = ConduitType.Liquid;
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.ViewMode = SimViewMode.LiquidVentMap;
			buildingDef.ObjectLayer = ObjectLayer.LiquidConduitConnection;
			buildingDef.SceneLayer = Grid.SceneLayer.LiquidConduitBridges;
			buildingDef.AudioCategory = "Metal";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.PermittedRotations = PermittedRotations.R360;
			buildingDef.UtilityInputOffset = new CellOffset(0, 0);
			buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, ID);
			return buildingDef;
		}

		private void AttachPort(GameObject go)
		{
			go.AddComponent<ConduitSecondaryOutput>().portInfo = this.secondaryPort;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			base.DoPostConfigurePreview(def, go);
			this.AttachPort(go);
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
			var logic = go.AddOrGet<FlowSplitter>();
			logic.type = ConduitType.Liquid;
			logic.SecondaryPort = secondaryPort;
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			this.AttachPort(go);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			Object.DestroyImmediate((Object)go.GetComponent<RequireInputs>());
			Object.DestroyImmediate((Object)go.GetComponent<ConduitConsumer>());
			Object.DestroyImmediate((Object)go.GetComponent<ConduitDispenser>());
			BuildingTemplates.DoPostConfigure(go);
		}
	}
}