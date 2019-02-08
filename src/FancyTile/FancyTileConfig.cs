using TUNING;
using UnityEngine;

namespace FancyTile
{
	public class FancyTileConfig : IBuildingConfig
	{
		public static string Id = "FancyTile";
		public const string DisplayName = "Fancy Tile";
		public static string Description = string.Empty;
		public const string Effect = "Used as floor and wall tile to build rooms.\n\nSignificantly increases Duplicant runspeed.";

		private static readonly int BlockTileConnectorId = Hash.SDBMLower("tiles_bunker_tops");

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 1,
				height: 1,
				anim: "floor_moulding_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER2,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
				construction_materials: MATERIALS.PRECIOUS_ROCKS,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER2,
				build_location_rule: BuildLocationRule.Tile,
				decor: DECOR.BONUS.TIER1,
				noise: NOISE_POLLUTION.NONE);

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
			buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_moulding");
			buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_moulding_place");
			buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
			buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_bunker_tops_decor_info");
			buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_bunker_tops_decor_place_info");

			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

			go.AddOrGet<SimCellOccupier>().doReplaceElement = true;
			go.AddOrGet<SimCellOccupier>().movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT.BONUS_4;
			go.AddOrGet<TileTemperature>();
			go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = BlockTileConnectorId; 
			go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
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
