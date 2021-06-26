using HarmonyLib;
using UnityEngine;

namespace OilWellAnyWater
{
	public class OilWellAnyWaterPatches
	{
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

				var conduitConsumer = go.AddOrGet<ConduitConsumer>();
				conduitConsumer.capacityTag = GameTags.AnyWater;
			}
		}
	}
}