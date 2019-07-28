using STRINGS;
using TUNING;
using UnityEngine;
using BUILDINGS = TUNING.BUILDINGS;

namespace BuildablePOIProps
{
	public class TableWithChairsConfig : IBuildingConfig
	{
		public const string Id = "TableWithChairs";
		public const string DisplayName = "Table";
		public const string Description = "A table and some chairs. Perfect for a break.";
		public static string Effect = $"Increases {UI.FormatAsLink("Decor", "DECOR")}, contributing to {UI.FormatAsLink("Morale", "MORALE")}.";

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 3,
				height: 1,
				anim: "table_breakroom_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER1,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				construction_materials: MATERIALS.PLASTICS,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				build_location_rule: BuildLocationRule.OnFloor,
				decor: DECOR.BONUS.TIER3,
				noise: NOISE_POLLUTION.NONE);

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

		}
	}
}
