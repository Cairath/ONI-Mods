using Harmony;
using UnityEngine;

namespace OilWellAnyWater
{
	public class OilWellAnyWaterPatches
	{
		[HarmonyPatch(typeof(OilWellCapConfig), "ConfigureBuildingTemplate")]
		public class OilWellCapConfig_ConfigureBuildingTemplate_Patch
		{
			private static void Postfix(ref GameObject go)
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
