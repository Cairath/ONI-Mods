using Harmony;
using static CaiLib.Logger.Logger;
using static CaiLib.Utils.BuildingUtils;

namespace SteelLadder
{
	public static class SteelLadderPatches
	{
		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch("LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				AddBuildingStrings(SteelLadderConfig.Id, SteelLadderConfig.DisplayName, SteelLadderConfig.Description, SteelLadderConfig.Effect);
				AddBuildingToPlanScreen("Base", SteelLadderConfig.Id, LadderFastConfig.ID);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch(nameof(Db.Initialize))]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				AddBuildingToTechnology("Luxury", SteelLadderConfig.Id);
			}
		}
	}
}