using TUNING;
using UnityEngine;

namespace PalmeraTree
{
	public class TrellisConfig : IBuildingConfig
	{
		public const string ID = "Trellis";

		public override BuildingDef CreateBuildingDef()
		{
			string id = ID;
			int width = 2;
			int height = 3;
			string anim = "planttrellis_kanim";
			int hitpoints = 100;
			float construction_time = 30f;
			float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
			string[] farmable = MATERIALS.FARMABLE;
			float melting_point = 1600f;


			BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time,
				tieR2, farmable, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
			buildingDef.DefaultAnimState = "on_2";
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.Overheatable = false;
			buildingDef.IsFoundation = false;
			buildingDef.TileLayer = ObjectLayer.FoundationTile;
			buildingDef.ReplacementLayer = ObjectLayer.ReplacementTile;
			buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingBack;
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.SceneLayer = Grid.SceneLayer.TileFront;
			buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
			buildingDef.isSolidTile = false;
			buildingDef.DragBuild = false;
			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
			go.AddOrGet<SimCellOccupier>().doReplaceElement = true;
			go.AddOrGet<TileTemperature>();
			BuildingTemplates.CreateDefaultStorage(go, false).SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
			PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
			plantablePlot.occupyingObjectRelativePosition = new Vector3(0.0f, 0f, 0.0f);
			plantablePlot.AddDepositTag(GameTagsRanching.CropSeed2TileWide);
			plantablePlot.SetFertilizationFlags(true, false);
			plantablePlot.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Top);
			Prioritizable.AddRef(go);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			BuildingTemplates.DoPostConfigure(go);
		}

	}
}