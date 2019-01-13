using TUNING;
using UnityEngine;

namespace BuildablePOIProps.Clock
{
	public class ClockConfig : IBuildingConfig
	{
		public const string Id = "Clock";
		public const string DisplayName = "Clock";
		public const string Description = "A simple wall clock.";
		public const string Effect = "A pretty clock for your wall.";

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 1,
				height: 1,
				anim: "clock_poi_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER0,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER0,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER0,
				construction_materials: MATERIALS.PLASTICS,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER0,
				build_location_rule: BuildLocationRule.Anywhere,
				decor: new EffectorValues { amount = 5, radius = 6 },
				noise: NOISE_POLLUTION.NONE);

			buildingDef.Floodable = false;
			buildingDef.Overheatable = false;
			buildingDef.AudioCategory = "Plastic";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.ViewMode = OverlayModes.Decor.ID;
			buildingDef.SceneLayer = Grid.SceneLayer.Paintings;
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
