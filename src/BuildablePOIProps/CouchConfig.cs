using STRINGS;
using TUNING;
using UnityEngine;
using BUILDINGS = TUNING.BUILDINGS;

namespace BuildablePOIProps
{
	public class CouchConfig : IBuildingConfig
	{
		public const string Id = "Couch";
		public const string DisplayName = "Couch";
		public const string Description = "Perfect for some relaxation. Comfier than it looks!";
		public static string Effect = $"Increases {UI.FormatAsLink("Decor", "DECOR")}, contributing to {UI.FormatAsLink("Morale", "MORALE")}.";

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 4,
				height: 2,
				anim: "gravitas_couch_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER2,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				construction_mass: new[] { 200f, 20f },
				construction_materials: new[] { MATERIALS.PLASTIC, GameTags.BuildingFiber.ToString() },
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				build_location_rule: BuildLocationRule.OnFloor,
				decor: BUILDINGS.DECOR.BONUS.TIER3,
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
