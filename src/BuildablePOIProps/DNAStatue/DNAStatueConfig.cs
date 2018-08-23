using TUNING;
using UnityEngine;

namespace BuildablePOIProps.DNAStatue
{
    public class DNAStatueConfig : IBuildingConfig
	{
		public const string ID = "DNAStatue";

		public override BuildingDef CreateBuildingDef()
		{
			string id = ID;
			int width = 5;
			int height = 9;
			string anim = "gravitas_statue_kanim";
			int hitpoints = 500;
			float construction_time = 500f;
			float[] construction_mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
			string[] construction_materials = MATERIALS.RAW_MINERALS;
			float melting_point = 800f;

			BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
			EffectorValues none = NOISE_POLLUTION.NONE;

			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time,
				construction_mass, construction_materials, melting_point, build_location_rule, new EffectorValues { amount = 100, radius = 7 }, none);
			buildingDef.Floodable = true;
			buildingDef.Overheatable = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.ViewMode = SimViewMode.Decor;
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