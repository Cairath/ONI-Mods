using TUNING;
using UnityEngine;

namespace BuildablePOIProps.Couch
{
    public class CouchConfig : IBuildingConfig
	{
		public const string ID = "Couch";

		public override BuildingDef CreateBuildingDef()
		{
			string id = ID;
			int width = 4;
			int height = 2;
			string anim = "gravitas_couch_kanim";
			int hitpoints = 50;
			float construction_time = 20f;
			float[] construction_mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
			string[] construction_materials = MATERIALS.PLASTICS;
			float melting_point = 1600f;

			BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
			EffectorValues none = NOISE_POLLUTION.NONE;

			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time,
				construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER3, none);
			buildingDef.Floodable = true;
			buildingDef.Overheatable = false;
			buildingDef.AudioCategory = "Plastic";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.ViewMode = SimViewMode.Decor;
			buildingDef.SceneLayer = Grid.SceneLayer.Building;
			buildingDef.DefaultAnimState = "off";
			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddPrefabTag(GameTags.Decoration);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			BuildingTemplates.DoPostConfigure(go);
		}
	}
}