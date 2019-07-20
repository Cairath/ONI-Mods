using STRINGS;
using TUNING;
using UnityEngine;
using BUILDINGS = TUNING.BUILDINGS;

namespace WirelessAutomation
{
	public class WirelessAutomationEmitterConfig : IBuildingConfig
	{
		public static string Id = "WirelessAutomationEmitter";
		public const string DisplayName = "Wireless Automation Emitter";
		public const string Description = "Emits received signal that can be listened to by Wireless Receivers.";
		public const string Effect = "The emitter transmits received signal wirelessly on the chosen channel.";

		public const string PortOn = "When Active, emits an Active signal wirelessly on the specified channel";
		public const string PortOff = "When Standby, emits a Standby signal wirelessly on the specified channel";

		private static readonly LogicPorts.Port InputPort = LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID,
			new CellOffset(0, 0), UI.LOGIC_PORTS.CONTROL_OPERATIONAL, PortOn, PortOff, true);

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 1,
				height: 1,
				anim: "critter_sensor_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER1,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
				construction_materials: MATERIALS.REFINED_METALS,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				build_location_rule: BuildLocationRule.Anywhere,
				decor: DECOR.NONE,
				noise: NOISE_POLLUTION.NONE);

			buildingDef.Overheatable = false;
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.ViewMode = OverlayModes.Logic.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.SceneLayer = Grid.SceneLayer.Building;

			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 100f;
			buildingDef.SelfHeatKilowattsWhenActive = 0f;
			buildingDef.PowerInputOffset = new CellOffset(0, 0);

			SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER0);
			SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER0);

			GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, Id);

			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, new[] { InputPort });
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, new[] { InputPort });
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, new[] { InputPort });

			go.AddOrGet<WirelessAutomationEmitter>().EmitChannel = 0;
			go.AddOrGet<LogicOperationalController>().unNetworkedValue = 0;
		}
	}
}
