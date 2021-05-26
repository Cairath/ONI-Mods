using CaiLib.Utils;
using Harmony;
using STRINGS;
using UnityEngine;
using static CaiLib.Logger.Logger;
using static CaiLib.Utils.BuildingUtils;
using static CaiLib.Utils.StringUtils;

namespace NotificationTrigger
{
	public static class NotificationTriggerPatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit();
			}
		}

		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				AddBuildingStrings(NotificationTriggerConfig.Id, NotificationTriggerConfig.DisplayName, NotificationTriggerConfig.Description, NotificationTriggerConfig.Effect);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Automation, NotificationTriggerConfig.Id);

				Strings.Add($"STRINGS.MISC.STATUSITEMS.{NotificationTriggerConfig.Id.ToUpperInvariant()}.NAME", "Notification Triggered");
				Strings.Add($"STRINGS.MISC.STATUSITEMS.{NotificationTriggerConfig.Id.ToUpperInvariant()}.TOOLTIP", "Custom notification has been triggered");
				Strings.Add($"STRINGS.MISC.STATUSITEMS.{NotificationTriggerConfig.Id.ToUpperInvariant()}.NOTIFICATION_TOOLTIP", "Custom notification has been triggered");
			}
		}

		[HarmonyPatch(typeof(BasicSingleHarvestPlantConfig))]
		[HarmonyPatch("OnPrefabInit")]
		public static class Plant
		{
			public static void Postfix(ref GameObject prefab)
			{
				prefab.GetComponent<PressureVulnerable>()
					
					.safe_atmospheres
					.Add(ElementLoader.FindElementByHash(SimHashes.SourGas));
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				AddBuildingToTechnology(GameStrings.Technology.Computers.SmartHome, NotificationTriggerConfig.Id);
			}
		}

		[HarmonyPatch(typeof(DetailsScreen))]
		[HarmonyPatch(nameof(DetailsScreen.SetTitle))]
		[HarmonyPatch(new[] { typeof(int) })]
		public static class DetailsScreen_SetTitle_Patch
		{
			public static void Postfix(DetailsScreen __instance, EditableTitleBar ___TabTitle)
			{
				var target = __instance.target;
				var notificationTrigger = target.gameObject.GetComponent<NotificationTrigger>();

				if (notificationTrigger == null)
					return;

				___TabTitle.SetUserEditable(true);
				___TabTitle.SetSubText(string.Empty);

				if (UI.StripLinkFormatting(target.GetProperName()) != NotificationTriggerConfig.DisplayName)
				{
					___TabTitle.SetSubText(NotificationTriggerConfig.DisplayName);
				}
			}
		}

		[HarmonyPatch(typeof(DetailsScreen))]
		[HarmonyPatch("OnNameChanged")]
		public static class DetailsScreen_OnNameChanged_Patch
		{
			public static void Postfix(string newName, DetailsScreen __instance, EditableTitleBar ___TabTitle)
			{
				var notificationTrigger = __instance.target.gameObject.GetComponent<NotificationTrigger>();

				if (notificationTrigger == null)
					return;

				//notificationTrigger.SetName(newName);
			}
		}

		[HarmonyPatch(typeof(EditableTitleBar))]
		[HarmonyPatch("OnEndEdit")]
		public class EditableTitleBar_OnEndEdit_Patch
		{
			public static void Prefix(ref string finalStr)
			{
				if (string.IsNullOrEmpty(finalStr))
				{
					finalStr = NotificationTriggerConfig.DisplayName;
				}
			}
		}

		[HarmonyPatch(typeof(EditableTitleBar))]
		[HarmonyPatch("SetEditingState")]
		public class EditableTitleBar_SetEditingState_Patch
		{
			public static void Postfix(EditableTitleBar __instance)
			{
				__instance.inputField.characterLimit = 50;
			}
		}
	}
}