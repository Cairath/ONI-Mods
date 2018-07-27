using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUNING;
using UnityEngine;

namespace SteelLadder
{
	public class SteelLadderConfig : IBuildingConfig
	{
		public const string ID = "SteelLadder";

		public override BuildingDef CreateBuildingDef()
		{
			string id = "SteelLadder";
			int width = 1;
			int height = 1;
			string anim = "ladder_poi_kanim";
			int hitpoints = 10;
			float construction_time = 10f;
			float[] construction_mass = new float[2]
			{
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0],
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0]
			};
			string[] construction_materials = new string[2]
			{
				SimHashes.Steel.ToString(),
				MATERIALS.PLASTIC
			};
			float melting_point = 1600f;
			BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time,
				construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none,
				0.2f);
			buildingDef.Floodable = false;
			buildingDef.Overheatable = false;
			buildingDef.Entombable = false;
			buildingDef.AudioCategory = "Plastic";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.DragBuild = true;
			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			Ladder ladder = go.AddOrGet<Ladder>();
			ladder.upwardsMovementSpeedMultiplier = 1.2f;
			ladder.downwardsMovementSpeedMultiplier = 1.2f;
			go.AddOrGet<AnimTileable>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			BuildingTemplates.DoPostConfigure(go);
		}
	}
}
