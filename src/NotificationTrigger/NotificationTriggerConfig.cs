using STRINGS;
using TUNING;
using UnityEngine;
using BUILDINGS = TUNING.BUILDINGS;

namespace NotificationTrigger
{
	public class NotificationTriggerConfig : IBuildingConfig
	{
		public const string Id = "NotificationTrigger";
		public const string DisplayName = "Notification Trigger";

		public const string Description = "Click the pencil icon to set your notification text.\n" +
		                                  "Possible customization:\n" +
		                                  "- none: Neutral Notification\n" +
		                                  "- [!]: Yellow Notification\n" +
		                                  "- [!!]: Danger Notification\n" +
		                                  "- [!!!]: Danger Notification, will pause the game when triggered\n\n" +
		                                  "To use customization simply include its symbol when you name your notification, for example: \"[!!] Out of coal\"\n\n" +
		                                  "PS: You don't have to type 'none'.";

		public static string Effect = $"Displays custom notification when it receives a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)}.";

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 1,
				height: 1,
				anim: "switchpower_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER1,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
				construction_materials: MATERIALS.REFINED_METALS,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				build_location_rule: BuildLocationRule.Anywhere,
				decor: DECOR.NONE,
				noise: NOISE_POLLUTION.NONE);

			buildingDef.Floodable = false;
			buildingDef.Overheatable = false;
			buildingDef.Entombable = false;
			buildingDef.ViewMode = OverlayModes.Logic.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.SceneLayer = Grid.SceneLayer.Building;
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.DefaultAnimState = "off";

			SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER1);
			SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER1);
			GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, Id);

			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<NotificationTrigger>();
			GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);

			go.AddOrGet<LogicOperationalController>().unNetworkedValue = 0;
		}
	}
}
