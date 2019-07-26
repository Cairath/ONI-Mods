using STRINGS;
using TUNING;
using UnityEngine;
using BUILDINGS = TUNING.BUILDINGS;

namespace WirelessAutomation
{
	public class WirelessSignalReceiverConfig : IBuildingConfig
	{
		public static string Id = "WirelessSignalReceiver";
		public const string DisplayName = "Wireless Signal Receiver";
		public static string Description = "";//" $"Listens to signals emitted by {UI.FormatAsLink(WirelessSignalEmitterConfig.DisplayName, WirelessSignalEmitterConfig.Id)}.";

		public static string Effect = $"Paired with {WirelessSignalEmitterConfig.DisplayName} allows transferring automation signals wirelessly on multiple channels." +
									  $"\n\nThere can be multiple {UI.FormatAsLink(WirelessSignalReceiverConfig.DisplayName, WirelessSignalReceiverConfig.Id)}" +
									  $", but only one {WirelessSignalEmitterConfig.DisplayName} on the same channel.";

		public static string PortOn = $"Sends {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if somewhere on the map there is a " +
			  $"{WirelessSignalEmitterConfig.DisplayName} emitting {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} on the same channel.";

		public static string PortOff = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

		private static readonly LogicPorts.Port OutputPort = LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID,
			new CellOffset(0, 0), UI.LOGIC_PORTS.CONTROL_OPERATIONAL, PortOn, PortOff, true);

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 1,
				height: 1,
				anim: "wifi_receiver_kanim",
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

			GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, Id);

			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, OutputPort);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, OutputPort);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, OutputPort);

			go.AddOrGet<WirelessSignalReceiver>().ReceiveChannel = 0;
		}
	}
}