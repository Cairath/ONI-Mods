using TUNING;
using UnityEngine;

namespace BuildablePOIProps.Chair
{
    public class ChairLeftConfig : IBuildingConfig
	{
		public const string ID = "ChairLeft";

		public override BuildingDef CreateBuildingDef()
		{
			string id = ID;
			int width = 2;
			int height = 2;
			string anim = "gravitas_chair_kanim";
			int hitpoints = 50;
			float construction_time = 20f;
			 float[] construction_mass = new[] { 100f, 10f };
			string[] construction_materials = new[] { MATERIALS.PLASTIC, GameTags.BuildingFiber.ToString() };
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
			buildingDef.ViewMode = OverlayModes.Decor.ID;
			buildingDef.SceneLayer = Grid.SceneLayer.Building;
			buildingDef.DefaultAnimState = "off";
			buildingDef.PermittedRotations = PermittedRotations.FlipH;
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