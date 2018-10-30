using Harmony;
using UnityEngine;

namespace OilWellAnyWater
{
	public class OilWellAnyWaterMod
	{
		[HarmonyPatch(typeof(OilWellCapConfig), "ConfigureBuildingTemplate")]
		public class OilWellAnyWaterModPatch
		{
			private static void Postfix(ref GameObject go)
			{
				ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
				elementConverter.consumedElements = new[]
				{
					new ElementConverter.ConsumedElement(GameTags.AnyWater, 1f)
				};
			}
		}
	}
}
