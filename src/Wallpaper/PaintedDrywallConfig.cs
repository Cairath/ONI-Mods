﻿using STRINGS;
using TUNING;
using UnityEngine;
using BUILDINGS = TUNING.BUILDINGS;

namespace Wallpaper
{
	public class PaintedDrywallConfig : IBuildingConfig
	{
		public const string Id = "PaintedDrywall";
		public const string DisplayName = "Painted Drywall";
		public const string Description = "Bring a little more variety to your base. Now with colors!";
		public static string Effect = $"Increases {UI.FormatAsLink("Decor", "DECOR")}, contributing to {UI.FormatAsLink("Morale", "MORALE")}.";

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 1,
				height: 1,
				anim: "walls_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER0,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER0,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER_TINY,
				construction_materials: MATERIALS.ANY_BUILDABLE,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER0,
				build_location_rule: BuildLocationRule.Anywhere,
				decor: new EffectorValues { amount = 5, radius = 1 },
				noise: NOISE_POLLUTION.NONE);

			buildingDef.Floodable = false;
			buildingDef.Overheatable = false;
			buildingDef.Entombable = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.DefaultAnimState = "off";
			buildingDef.ObjectLayer = ObjectLayer.Backwall;
			buildingDef.SceneLayer = Grid.SceneLayer.LogicGatesFront;

			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;

			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);
			BuildingTemplates.DoPostConfigure(go);
		}
	}
}
