using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;
using BUILDINGS = TUNING.BUILDINGS;

namespace WirelessAutomation
{
	public class WirelessSignalEmitterConfig : IBuildingConfig
	{
		public static string Id = "WirelessSignalEmitter";
		public const string DisplayName = "Wireless Signal Emitter";
		public static string Description = $"Emits signal that can be listened to by {UI.FormatAsLink(WirelessSignalReceiverConfig.DisplayName, WirelessSignalReceiverConfig.Id)}.";
		public static string Effect = $"Paired with {UI.FormatAsLink(WirelessSignalReceiverConfig.DisplayName, WirelessSignalReceiverConfig.Id)} allows transferring automation " +
		                              $"signals wirelessly on multiple channels." +
		                              $"\n\nThere can be multiple {UI.FormatAsLink(WirelessSignalReceiverConfig.DisplayName, WirelessSignalReceiverConfig.Id)}" +
		                              $", but only one {UI.FormatAsLink(WirelessSignalEmitterConfig.DisplayName, WirelessSignalEmitterConfig.Id)} on the same channel.";

		public static string PortOn
			= $"{UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)}:" +
			  $" emit {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} " +
			  $"signal wirelessly allowing it to be received by {UI.FormatAsLink(WirelessSignalReceiverConfig.DisplayName, WirelessSignalReceiverConfig.Id)}";

		public static string PortOff
			= $"{UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby)}:" +
			  $" emit {UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby)} " +
			  $"signal wirelessly allowing it to be received by {UI.FormatAsLink(WirelessSignalReceiverConfig.DisplayName, WirelessSignalReceiverConfig.Id)}";

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 1,
				height: 1,
				anim: "wifi_emitter_kanim",
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
			buildingDef.EnergyConsumptionWhenActive = 50f;
			buildingDef.SelfHeatKilowattsWhenActive = 0f;
			buildingDef.PowerInputOffset = new CellOffset(0, 0);
			buildingDef.LogicInputPorts = new List<LogicPorts.Port>
			{
				LogicPorts.Port.RibbonInputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0),
					UI.LOGIC_PORTS.CONTROL_OPERATIONAL, PortOn, PortOff, true)
			};

			GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, Id);

			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<WirelessSignalEmitter>().EmitChannel = 0;
			go.AddOrGet<LogicOperationalController>().unNetworkedValue = 0;
		}
	}
}
