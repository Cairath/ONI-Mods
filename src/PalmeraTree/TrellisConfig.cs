using TUNING;
using UnityEngine;

namespace PalmeraTree
{
	public class TrellisConfig : IBuildingConfig
	{
		public const string ID = "Trellis";

		public override BuildingDef CreateBuildingDef()
		{
			int width = 2;
			int height = 1;
			string anim = "planttrellis_kanim";
			int hitpoints = 100;
			float construction_time = 30f;
			float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
			string[] allMetals = MATERIALS.FARMABLE;
			float melting_point = 1600f;
			BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, width, height, anim, hitpoints, construction_time,
				tieR2, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
			buildingDef.DefaultAnimState = "on_1";
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.Overheatable = false;
			buildingDef.IsFoundation = false;
			buildingDef.ObjectLayer = ObjectLayer.Backwall;
			buildingDef.UseStructureTemperature = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.AudioSize = "small";
			buildingDef.isSolidTile = false;
			buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
			return buildingDef;
		}

		

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<Storage>();
			PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
			plantablePlot.AddDepositTag(Utils.CropSeed2TileWide);
			plantablePlot.occupyingObjectRelativePosition.y = 0f;
			plantablePlot.SetFertilizationFlags(true, false);
			BuildingTemplates.CreateDefaultStorage(go, false).SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
			go.AddOrGet<AnimTileable>();
			go.AddOrGet<DropAllWorkable>();

			Prioritizable.AddRef(go);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			BuildingTemplates.DoPostConfigure(go);
			PlantablePlot plantablePlot = go.GetComponent<PlantablePlot>();
			plantablePlot.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Top);
		}
	}
}