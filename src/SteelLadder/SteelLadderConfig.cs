using TUNING;
using UnityEngine;

namespace SteelLadder
{
	public class SteelLadderConfig : IBuildingConfig
	{
		public const string Id = "SteelLadder";
		public const string DisplayName = "Wallpaper";
		public const string Description = "Durable steel ladder.";
		public const string Effect = "Increases Duplicant climbing speed.";

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 1,
				height: 1,
				anim: "ladder_poi_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER2,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
				construction_materials: new[] { SimHashes.Steel.ToString() },
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER2,
				build_location_rule: BuildLocationRule.Anywhere,
				decor: DECOR.BONUS.TIER0,
				noise: NOISE_POLLUTION.NONE);

			buildingDef.Floodable = false;
			buildingDef.Overheatable = false;
			buildingDef.Entombable = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.DragBuild = true;


			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);

			var ladder = go.AddOrGet<Ladder>();
			ladder.upwardsMovementSpeedMultiplier = 1.5f;
			ladder.downwardsMovementSpeedMultiplier = 1.5f;

			go.AddOrGet<AnimTileable>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			BuildingTemplates.DoPostConfigure(go);
		}
	}
}
