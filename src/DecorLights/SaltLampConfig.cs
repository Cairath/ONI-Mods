using TUNING;
using UnityEngine;

namespace DecorLights
{
	public class SaltLampConfig : IBuildingConfig
	{
		public const string ID = "SaltLamp";

		public override BuildingDef CreateBuildingDef()
		{
			string id = ID;
			int width = 1;
			int height = 2;
			string anim = "saltlamp_kanim";
			int hitpoints = 10;
			float construction_time = 10f;
			float[] tieR1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
			string[] allMetals = MATERIALS.TRANSPARENTS;
			float melting_point = 800f;
			BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time,
				tieR1, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER5, none, 0.2f);
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 4f;
			buildingDef.SelfHeatKilowattsWhenActive = 0.3f;
			buildingDef.ViewMode = SimViewMode.Light;
			buildingDef.AudioCategory = "Metal";
			return buildingDef;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
			lightShapePreview.lux = 1200;
			lightShapePreview.radius = 5f;
			lightShapePreview.shape = LightShape.Circle;
			lightShapePreview.offset = new CellOffset((int)def.BuildingComplete.GetComponent<Light2D>().Offset.x,
				(int)def.BuildingComplete.GetComponent<Light2D>().Offset.y);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<EnergyConsumer>();
			go.AddOrGet<LoopingSounds>();
			Light2D light2D = go.AddOrGet<Light2D>();
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
			go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn)(game_object =>
			   new LightController.Instance(game_object.GetComponent<KPrefabID>()).StartSM());
		}
	}
}