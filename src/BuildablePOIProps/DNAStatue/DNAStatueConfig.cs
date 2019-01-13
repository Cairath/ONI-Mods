using TUNING;
using UnityEngine;

namespace BuildablePOIProps.DNAStatue
{
    public class DNAStatueConfig : IBuildingConfig
	{
		public const string Id = "DNAStatue";
		public const string DisplayName = "DNA Statue";
		public const string Description = "An enormous statue of a DNA chain.";
		public const string Effect = "Big and difficult to build.";

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 5,
				height: 9,
				anim: "gravitas_statue_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER4,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER6,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER7,
				construction_materials: MATERIALS.RAW_MINERALS,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				build_location_rule: BuildLocationRule.OnFloor,
				decor: DECOR.BONUS.TIER7,
				noise: NOISE_POLLUTION.NONE);

			buildingDef.Floodable = true;
			buildingDef.Overheatable = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.ViewMode = OverlayModes.Decor.ID;
			buildingDef.SceneLayer = Grid.SceneLayer.Building;
			buildingDef.DefaultAnimState = "off";

			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			BuildingTemplates.DoPostConfigure(go);
		}
	}
}
