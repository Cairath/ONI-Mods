using TUNING;
using UnityEngine;

namespace DecorLights
{
	public class BowlLampConfig : IBuildingConfig
	{
		public const string Id = "BowlLamp";
		public const string DisplayName = "Bowl Lamp";
		public static string Description = STRINGS.BUILDINGS.PREFABS.CEILINGLIGHT.DESC;
		public static string Effect = STRINGS.BUILDINGS.PREFABS.CEILINGLIGHT.EFFECT;

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 1,
				height: 1,
				anim: "ceilinglight_pretty_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER0,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
				construction_materials: MATERIALS.TRANSPARENTS,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER0,
				build_location_rule: BuildLocationRule.OnCeiling,
				decor: DECOR.BONUS.TIER5,
				noise: NOISE_POLLUTION.NONE);

			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 2f;
			buildingDef.SelfHeatKilowattsWhenActive = 0.25f;
			buildingDef.ViewMode = OverlayModes.Light.ID;
			buildingDef.AudioCategory = "Metal";

			return buildingDef;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			var lightShapePreview = go.AddComponent<LightShapePreview>();
			lightShapePreview.lux = 1800;
			lightShapePreview.radius = 9f;
			lightShapePreview.shape = LightShape.Cone;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<LoopingSounds>();

			var light2D = go.AddOrGet<Light2D>();
			light2D.overlayColour = LIGHT2D.CEILINGLIGHT_OVERLAYCOLOR;
			light2D.Color = LIGHT2D.CEILINGLIGHT_COLOR;
			light2D.Range = 9f;
			light2D.Angle = 3f;
			light2D.Direction = LIGHT2D.CEILINGLIGHT_DIRECTION;
			light2D.Offset = LIGHT2D.CEILINGLIGHT_OFFSET;
			light2D.shape = LightShape.Cone;
			light2D.drawOverlay = true;
			light2D.Lux = 1800;

			go.GetComponent<KPrefabID>().prefabInitFn += gameObject => new CeilingLamp.Instance(gameObject.GetComponent<KPrefabID>()).StartSM();
		}
	}
}
