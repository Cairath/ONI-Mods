using TUNING;
using UnityEngine;

namespace BuildablePOIProps.ComputerDesk
{
	public class ComputerDeskConfig : IBuildingConfig
	{
		public const string Id = "ComputerDesk";
		public const string DisplayName = "Computer desk";
		public const string Description = "An intact office desk, decorated with several personal belongings and a barely functioning computer.";
		public const string Effect = "Does it work? Who knows.";

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 3,
				height: 2,
				anim: "setpiece_desk_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER1,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER0,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
				construction_materials: new[] { SimHashes.Steel.ToString() },
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				build_location_rule: BuildLocationRule.OnFloor,
				decor: BUILDINGS.DECOR.BONUS.TIER5,
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
