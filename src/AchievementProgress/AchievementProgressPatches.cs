﻿using System.Linq;
using HarmonyLib;

namespace AchievementProgress
{
	public class AchievementProgressPatches
	{
		public static PauseScreen Instance;

		[HarmonyPatch(typeof(PauseScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class PauseScreenOnPrefabInit
		{
			public static void Postfix(PauseScreen __instance)
			{
				var instance = Traverse.Create(__instance);
				var buttons = instance.Field("buttons").GetValue<KButtonMenu.ButtonInfo[]>();
				var buttonsList = buttons.ToList();
				Instance = __instance;

				buttonsList.Insert(buttonsList.Count - 2, new KButtonMenu.ButtonInfo("Achievement Progress", Action.NumActions,
					() => { AchievementProgressScreen.CreateScreen(Instance); }));

				instance.Field("buttons").SetValue(buttonsList.ToArray());

			}
		}
	}
}
