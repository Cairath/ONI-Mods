using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;
using KMod;
using static CaiLib.Logger.Logger;

namespace EnableAllModsButton
{
	public class EnableAllModsButtonPatches
	{
		private const string DisableAllButtonName = "DisableAllButton";
		private const string EnableAllButtonName = "EnableAllButton";

		private const string LocalId = "EnableAllModsButton";
		private const string SteamId = "1855960639";

		private static bool WasActive = false;

		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit();
			}
		}

		[HarmonyPatch(typeof(ModsScreen))]
		[HarmonyPatch("BuildDisplay")]
		public static class ModsScreen_OnSpawn_Patch
		{
			public static void Postfix(ModsScreen __instance, KButton ___workshopButton)
			{
				var buttons = ___workshopButton.transform.parent.GetComponentsInChildren<KButton>();
				var hasEnableAll = false;
				var hasDisableAll = false;

				foreach (var button in buttons)
				{
					if (button.name == EnableAllButtonName)
					{
						hasEnableAll = true;
					}

					if (button.name == DisableAllButtonName)
					{
						hasDisableAll = true;
					}
				}

				if (!hasDisableAll)
				{
					var disableAllButton = Util.KInstantiateUI<KButton>(___workshopButton.gameObject,
						___workshopButton.transform.parent.gameObject);
					disableAllButton.name = DisableAllButtonName;
					disableAllButton.transform.GetComponentInChildren<LocText>().text = "DISABLE ALL";
					disableAllButton.transform.SetAsFirstSibling();
					disableAllButton.gameObject.SetActive(true);
					disableAllButton.onClick += (() => ToggleAllMods(__instance, false));
				}

				if (!hasEnableAll)
				{
					var enableAllButton = Util.KInstantiateUI<KButton>(___workshopButton.gameObject,
						___workshopButton.transform.parent.gameObject);
					enableAllButton.name = EnableAllButtonName;
					enableAllButton.transform.GetComponentInChildren<LocText>().text = "ENABLE ALL";
					enableAllButton.transform.SetAsFirstSibling();
					enableAllButton.gameObject.SetActive(true);
					enableAllButton.onClick += (() => ToggleAllMods(__instance, true));
				}
			}
		}

		[HarmonyPatch(typeof(KMod.Manager))]
		[HarmonyPatch(nameof(KMod.Manager.HandleCrash))]
		public static class KModManager_HandleCrash_Patch
		{
			public static void Prefix(List<Mod> ___mods, out bool __state)
			{
				var mod = ___mods.FirstOrDefault(m => m.label.id == LocalId || m.label.id == SteamId);

				__state = mod != null && mod.enabled;
			}

			public static void Postfix(KMod.Manager __instance, List<Mod> ___mods, ref bool ___dirty)
			{
				var mod = ___mods.FirstOrDefault(m => m.label.id == LocalId || m.label.id == SteamId);
				if (mod == null)
					return;

				mod.enabled = true;
				___dirty = true;

				__instance.Update(__instance);
			}
		}

		[HarmonyPatch(typeof(Mod))]
		[HarmonyPatch(nameof(Mod.Crash))]
		public static class Mod_Crash_Patch
		{
			public static bool Prefix(Mod __instance)
			{
				return __instance.label.id != SteamId && __instance.label.id != LocalId;
			}
		}

		[HarmonyPatch(typeof(KMod.Manager))]
		[HarmonyPatch("DevRestartDialog")]
		public static class Manager_DevRestartDialog_Patch
		{
			public static void Prefix(List<Mod> ___mods)
			{
				if (___mods.Any(m => m.label.id == SteamId || m.label.id == LocalId))
					WasActive = true;
			}
		}

		[HarmonyPatch(typeof(KMod.Manager))]
		[HarmonyPatch(nameof(KMod.Manager.Update))]
		[HarmonyPatch(new Type[] { typeof(object) })]
		public static class Manager_Update_Patch
		{
			public static void Prefix(List<Mod> ___mods)
			{
				if (WasActive == false)
					return;

				var mod = ___mods.FirstOrDefault(m => m.label.id == SteamId || m.label.id == LocalId);
				if (mod == null)
					return;

				mod.enabled = true;
				WasActive = false;
			}
		}

		public static void ToggleAllMods(ModsScreen modsScreen, bool enable)
		{
			var modManager = Global.Instance.modManager;
			foreach (var mod in modManager.mods)
			{
				modManager.EnableMod(mod.label, enable, modsScreen);
				var toggles = modsScreen.GetComponentsInChildren<MultiToggle>();

				foreach (var toggle in toggles)
				{
					toggle.ChangeState(enable ? 1 : 0);
				}
			}
		}
	}
}
