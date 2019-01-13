using TUNING;
using UnityEngine;

namespace DecorLights
{
	public class SaltLampConfig : IBuildingConfig
	{
		public const string Id = "SaltLamp";
		public const string DisplayName = "Salt Lamp";
		public const string Description = "Fake salt. Not edible. Do not lick.";
		public static string Effect = STRINGS.BUILDINGS.PREFABS.CEILINGLIGHT.DESC;

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 1,
				height: 2,
				anim: "saltlamp_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER0,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
				construction_materials: MATERIALS.TRANSPARENTS,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER0,
				build_location_rule: BuildLocationRule.OnFloor,
				decor: DECOR.BONUS.TIER5,
				noise: NOISE_POLLUTION.NONE);

			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 4f;
			buildingDef.SelfHeatKilowattsWhenActive = 0.3f;
			buildingDef.ViewMode = OverlayModes.Light.ID;
			buildingDef.AudioCategory = "Metal";

			return buildingDef;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			var lightShapePreview = go.AddComponent<LightShapePreview>();
			lightShapePreview.lux = 1200;
			lightShapePreview.radius = 5f;
			lightShapePreview.shape = LightShape.Circle;
			lightShapePreview.offset = new CellOffset((int)def.BuildingComplete.GetComponent<Light2D>().Offset.x, (int)def.BuildingComplete.GetComponent<Light2D>().Offset.y);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<EnergyConsumer>();
			go.AddOrGet<LoopingSounds>();

			var light2D = go.AddOrGet<Light2D>();
			light2D.overlayColour = LIGHT2D.FLOORLAMP_OVERLAYCOLOR;
			light2D.Color = new Color(2, 2, 0);
			light2D.Range = 5f;
			light2D.Lux = 1200;
			light2D.Angle = 0.0f;
			light2D.Direction = LIGHT2D.FLOORLAMP_DIRECTION;
			light2D.Offset = new Vector2(0.05f, 1f);
			light2D.shape = LightShape.Circle;
			light2D.drawOverlay = true;

			BuildingTemplates.DoPostConfigure(go);

			go.AddOrGetDef<LightController.Def>();
		}
	}
}
