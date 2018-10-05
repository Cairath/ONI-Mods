using TUNING;
using UnityEngine;

namespace DecorLights
{
	public class WallLampConfig : IBuildingConfig
	{
		public const string ID = "WallLamp";

		public override BuildingDef CreateBuildingDef()
		{
			string id = ID;
			int width = 1;
			int height = 1;
			string anim = "walllamp_kanim";
			int hitpoints = 10;
			float construction_time = 10f;
			float[] tieR1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
			string[] allMetals = MATERIALS.ALL_METALS;
			float melting_point = 800f;
			BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time,
				tieR1, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 10f;
			buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
			buildingDef.ViewMode = SimViewMode.Light;
			buildingDef.AudioCategory = "Metal";
			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		}


		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
			lightShapePreview.lux = 1800;
			lightShapePreview.radius = 4f;
			lightShapePreview.shape = LightShape.Circle;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<LoopingSounds>();
			Light2D light2D = go.AddOrGet<Light2D>();
			light2D.overlayColour = LIGHT2D.CEILINGLIGHT_OVERLAYCOLOR;
			light2D.Color = new Color(0.5f, 0.5f, 0);
			light2D.Range = 4f;
			light2D.Angle = 0f;
			light2D.Direction = LIGHT2D.FLOORLAMP_DIRECTION;
			light2D.Offset = new Vector2(0f, 0.5f);
			light2D.shape = LightShape.Circle;
			light2D.drawOverlay = true;
			light2D.Lux = 1800;
			BuildingTemplates.DoPostConfigure(go);
			go.AddOrGetDef<LightController.Def>();
		}
	}
}
