using TUNING;
using UnityEngine;

namespace DecorLights
{
	public class CeilingLampConfig : IBuildingConfig
	{
		public const string ID = "CeilingLamp";

		public override BuildingDef CreateBuildingDef()
		{
			string id = ID;
			int width = 1;
			int height = 1;
			string anim = "setpiece_light_kanim";
			int hitpoints = 10;
			float construction_time = 10f;
			float[] tieR1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
			string[] allMetals = MATERIALS.ALL_METALS;
			float melting_point = 800f;
			BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time,
				tieR1, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 2f;
			buildingDef.SelfHeatKilowattsWhenActive = 0.25f;
			buildingDef.ViewMode = OverlayModes.Light.ID;
			buildingDef.AudioCategory = "Metal";
			return buildingDef;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
			lightShapePreview.lux = 1800;
			lightShapePreview.radius = 9f;
			lightShapePreview.shape = LightShape.Cone;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<LoopingSounds>();
			Light2D light2D = go.AddOrGet<Light2D>();
			light2D.overlayColour = LIGHT2D.CEILINGLIGHT_OVERLAYCOLOR;
			light2D.Color = LIGHT2D.CEILINGLIGHT_COLOR;
			light2D.Range = 9f;
			light2D.Angle = 3f;
			light2D.Direction = LIGHT2D.CEILINGLIGHT_DIRECTION;
			light2D.Offset = LIGHT2D.CEILINGLIGHT_OFFSET;
			light2D.shape = LightShape.Cone;
			light2D.drawOverlay = true;
			light2D.Lux = 1800;
			BuildingTemplates.DoPostConfigure(go);
			go.GetComponent<KPrefabID>().prefabInitFn += game_object => new CeilingLamp.Instance(game_object.GetComponent<KPrefabID>()).StartSM();
		}
	}
}
