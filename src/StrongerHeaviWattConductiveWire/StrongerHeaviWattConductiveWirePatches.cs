using System.Collections.Generic;
using Harmony;
using STRINGS;
using UnityEngine;
using static CaiLib.Logger.Logger;

namespace StrongerHeaviWattConductiveWire
{
	public static class StrongerHeaviWattConductiveWirePatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(WireRefinedHighWattageConfig))]
		[HarmonyPatch(nameof(WireRefinedHighWattageConfig.DoPostConfigureComplete))]
		[HarmonyPatch(new[] { typeof(GameObject) })]
		public static class WireRefinedHighWattageConfig_DoPostConfigureComplete_Patch
		{
			public static void Postfix(ref GameObject go)
			{
				go.GetComponent<Wire>().MaxWattageRating = Wire.WattageRating.Max50000;

				var descriptor = new Descriptor();
				descriptor.SetupDescriptor(
					string.Format(UI.BUILDINGEFFECTS.MAX_WATTAGE, GameUtil.GetFormattedWattage(Wire.GetMaxWattageAsFloat(Wire.WattageRating.Max50000))),
					string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.MAX_WATTAGE));

				go.GetComponent<Building>().Def.EffectDescription = new List<Descriptor> { descriptor };
			}
		}
	}
}
