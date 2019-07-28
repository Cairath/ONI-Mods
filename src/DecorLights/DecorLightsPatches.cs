using Harmony;
using CaiLib.Utils;
using static CaiLib.Logger.Logger;
using static CaiLib.Utils.BuildingUtils;
using static CaiLib.Utils.StringUtils;

namespace DecorLights
{
	public static class Mod_OnLoad
	{
		public static void OnLoad()
		{
			LogInit(ModInfo.Name, ModInfo.Version);
		}
	}

	public class DecorLightsPatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				AddBuildingStrings(LavaLampConfig.Id, LavaLampConfig.DisplayName, LavaLampConfig.Description, LavaLampConfig.Effect);
				AddBuildingStrings(SaltLampConfig.Id, SaltLampConfig.DisplayName, SaltLampConfig.Description, SaltLampConfig.Effect);
				AddBuildingStrings(CeilingLampConfig.Id, CeilingLampConfig.DisplayName, CeilingLampConfig.Description, CeilingLampConfig.Effect);

				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, LavaLampConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, SaltLampConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, CeilingLampConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				AddBuildingToTechnology(GameStrings.Technology.Decor.HomeLuxuries, LavaLampConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Decor.HomeLuxuries, SaltLampConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Decor.HomeLuxuries, CeilingLampConfig.Id);
			}
		}
	}
}
