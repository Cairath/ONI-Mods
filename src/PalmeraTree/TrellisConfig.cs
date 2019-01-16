using TUNING;
using UnityEngine;

namespace PalmeraTree
{
	public class TrellisConfig : IBuildingConfig
	{
		public const string Id = "Trellis";
		public const string Description = "Used to plant trees.";
		public const string Effect = "For when you want to grow your very own tree.";

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 2,
				height: 1,
				anim: "planttrellis_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER2,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				construction_materials: MATERIALS.FARMABLE,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				build_location_rule: BuildLocationRule.OnFloor,
				decor: DECOR.NONE,
				noise: NOISE_POLLUTION.NONE);

			buildingDef.DefaultAnimState = "on_1";
			buildingDef.Overheatable = true;
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
			var plantablePlot = go.AddOrGet<PlantablePlot>();
			plantablePlot.AddDepositTag(Utils.CropSeed2TileWide);
			plantablePlot.occupyingObjectRelativePosition.y = 0f;
			plantablePlot.SetFertilizationFlags(true, false);

			go.AddOrGet<Storage>();
			BuildingTemplates.CreateDefaultStorage(go).SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);

			go.AddOrGet<AnimTileable>();
			go.AddOrGet<DropAllWorkable>();

			Prioritizable.AddRef(go);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			BuildingTemplates.DoPostConfigure(go);

			go.GetComponent<PlantablePlot>().SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Top);
		}
	}
}
