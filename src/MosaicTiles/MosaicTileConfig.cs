using TUNING;
using UnityEngine;

namespace MosaicTiles
{
	public class MosaicTileConfig : IBuildingConfig
	{
		public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_mosaic_tops");
		public static string ID = "MosaicTile";

		public override BuildingDef CreateBuildingDef()
		{
			string id = MosaicTileConfig.ID;
			int width = 1;
			int height = 1;
			string anim = "floor_mesh_kanim";
			int hitpoints = 100;
			float construction_time = 50f;
			float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
			string[] construction_materials = new string[1]
			{
				SimHashes.Ceramic.ToString()
			};
			float melting_point = 800f;
			BuildLocationRule build_location_rule = BuildLocationRule.Tile;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time,
				tieR3, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER4, none, 0.2f);
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.Overheatable = false;
			buildingDef.IsFoundation = true;
			buildingDef.UseStructureTemperature = false;
			buildingDef.TileLayer = ObjectLayer.FoundationTile;
			buildingDef.ReplacementLayer = ObjectLayer.ReplacementTile;
			buildingDef.AudioCategory = "Metal";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.SceneLayer = Grid.SceneLayer.TileMain;		
			buildingDef.isKAnimTile = true;
			buildingDef.isSolidTile = true;
			buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_POI");
			buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_solid_place");
			buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
			buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_POI_tops_decor_info");
			buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_POI_tops_decor_info");
			buildingDef.ConstructionOffsetFilter = new CellOffset[1]
			{
				new CellOffset(0, -1)
			};

			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
			go.AddOrGet<SimCellOccupier>().doReplaceElement = true;
			go.AddOrGet<SimCellOccupier>().movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT.BONUS_3;
			go.AddOrGet<TileTemperature>();
			go.AddOrGet<KAnimGridTileVisualizer>();
			go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
			go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = MosaicTileConfig.BlockTileConnectorID;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			BuildingTemplates.DoPostConfigure(go);
			go.AddComponent<SimTemperatureTransfer>();
			go.GetComponent<Deconstructable>().allowDeconstruction = true;
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			go.AddOrGet<KAnimGridTileVisualizer>();
		}
	}
}
