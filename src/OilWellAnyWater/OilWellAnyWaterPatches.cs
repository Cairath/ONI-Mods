using Harmony;
using UnityEngine;
using static CaiLib.Logger.Logger;

namespace OilWellAnyWater
{
	public class OilWellAnyWaterPatches
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

		[HarmonyPatch(typeof(OilWellCapConfig))]
		[HarmonyPatch(nameof(OilWellCapConfig.ConfigureBuildingTemplate))]
		public class OilWellCapConfig_ConfigureBuildingTemplate_Patch
		{
			public static void Postfix(ref GameObject go)
			{
				var elementConverter = go.AddOrGet<ElementConverter>();
				elementConverter.consumedElements = new[]
				{
					new ElementConverter.ConsumedElement(GameTags.AnyWater, 1f)
				};
			}
		}
	}
}