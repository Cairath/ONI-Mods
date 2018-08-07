using TUNING;
using UnityEngine;

namespace BuildablePOIProps.Clock
{
	public class ClockConfig : IBuildingConfig
	{
		public const string ID = "Clock";

		public override BuildingDef CreateBuildingDef()
		{
			string id = ID;
			int width = 1;
			int height = 1;
			string anim = "clock_poi_kanim";
			int hitpoints = 10;
			float construction_time = 10f;
			float[] construction_mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
			string[] construction_materials = MATERIALS.PLASTICS;
			float melting_point = 1600f;
			BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
			
			EffectorValues none = NOISE_POLLUTION.NONE;

			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time,
				construction_mass, construction_materials, melting_point, build_location_rule, new EffectorValues() { amount = 5, radius = 6 }, none);
			buildingDef.Floodable = false;
			buildingDef.Overheatable = false;
			buildingDef.AudioCategory = "Plastic";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.ViewMode = SimViewMode.Decor;
			buildingDef.SceneLayer = Grid.SceneLayer.Paintings;
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