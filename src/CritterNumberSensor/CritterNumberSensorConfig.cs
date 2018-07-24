using OverlayModes;
using TUNING;
using UnityEngine;

namespace CritterNumberSensor
{
	public class CritterNumberSensorConfig : IBuildingConfig
	{
		public static string ID = "LogicCritterNumberSensor";

		public override BuildingDef CreateBuildingDef()
		{
			string id = CritterNumberSensorConfig.ID;
			int width = 1;
			int height = 1;
			string anim = "diseasesensor_kanim";
			int hitpoints = 30;
			float construction_time = 30f;
			float[] construction_mass = new float[2]
			{
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0],
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
			};
			string[] construction_materials = new string[2]
			{
				"RefinedMetal",
				"Plastic"
			};
			float melting_point = 1600f;
			BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
			buildingDef.Overheatable = false;
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.ViewMode = SimViewMode.Logic;
			buildingDef.AudioCategory = "Metal";
			buildingDef.SceneLayer = Grid.SceneLayer.Building;
			SoundEventVolumeCache.instance.AddVolume("diseasesensor_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
			SoundEventVolumeCache.instance.AddVolume("diseasesensor_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
			GeneratedBuildings.RegisterWithOverlay(Logic.HighlightItemIDs, CritterNumberSensorConfig.ID);

			return buildingDef;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, LogicSwitchConfig.OUTPUT_PORT);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, LogicSwitchConfig.OUTPUT_PORT);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			GeneratedBuildings.RegisterLogicPorts(go, LogicSwitchConfig.OUTPUT_PORT);
			CritterNumberSensor critterNumberSensor = go.AddOrGet<CritterNumberSensor>();
			critterNumberSensor.Threshold = 0.0f;
			critterNumberSensor.ActivateAboveThreshold = true;
			critterNumberSensor.manuallyControlled = false;
		}
	}
}