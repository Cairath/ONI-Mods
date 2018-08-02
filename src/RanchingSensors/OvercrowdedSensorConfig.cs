using OverlayModes;
using TUNING;
using UnityEngine;

namespace RanchingSensors
{
	public class OvercrowdedSensorConfig : IBuildingConfig
	{
		public static string ID = "OvercrowdedSensor";

		public override BuildingDef CreateBuildingDef()
		{
			string id = ID;
			int width = 1;
			int height = 1;
			string anim = "switchtimeofday_kanim";
			int hitpoints = 30;
			float construction_time = 30f;
			float[] construction_mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
			string[] construction_materials = MATERIALS.REFINED_METALS;
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
			SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
			SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
			GeneratedBuildings.RegisterWithOverlay(Logic.HighlightItemIDs, ID);

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
			CrittersAndEggsSensor crittersAndEggsSensor = go.AddOrGet<CrittersAndEggsSensor>();
			crittersAndEggsSensor.ActivateAboveThreshold = false;
			crittersAndEggsSensor.manuallyControlled = false;
		}
	}
}