using System.Linq;
using Database;
using Harmony;
using static CaiLib.Logger.Logger;

namespace AchievementProgress
{
	public class AchievementProgressPatches
	{
		public static PauseScreen Instance;

		public static float SustainableEnergyCurrent;
		public static bool SustainableEnergyUsedDisallowedBuilding;
		public static bool SustainableEnergyFailed;

		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

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

		[HarmonyPatch(typeof(ProduceXEngeryWithoutUsingYList))]
		[HarmonyPatch("Update")]
		public static class ProduceXEngeryWithoutUsingYList_Update
		{
			public static void Postfix(ColonyAchievementTracker __instance)
			{
				var controlVal = 240000f;

				var i = Traverse.Create(__instance);
				var goal = i.Field("amountToProduce").GetValue<float>();

				if (goal != controlVal) return;
				SustainableEnergyCurrent = i.Field("amountProduced").GetValue<float>();
				SustainableEnergyUsedDisallowedBuilding = i.Field("usedDisallowedBuilding").GetValue<bool>();
			}
		}

		[HarmonyPatch(typeof(ColonyAchievementTracker))]
		[HarmonyPatch("CheckAchievements")]
		public static class ColonyAchievementTracker_CheckAchievements
		{
			public static void Postfix(ColonyAchievementTracker __instance)
			{
				SustainableEnergyFailed = __instance.achievements["Generate240000kJClean"].failed;
			}
		}
	}
}
