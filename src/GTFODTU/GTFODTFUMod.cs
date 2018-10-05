using System;
using System.Collections.Generic;
using Harmony;
using STRINGS;

namespace GTFODTU
{
	public class GTFODTU
	{
		[HarmonyPatch(typeof(Building), "EffectDescriptors")]
		public class BuildingPatch
		{
			private static void Postfix(BuildingDef def, ref Building __instance, ref List<Descriptor> __result)
			{
				List<Descriptor> descriptorList = new List<Descriptor>();
				if (def.EffectDescription != null)
					descriptorList.AddRange(def.EffectDescription);
				if (def.GeneratorWattageRating > 0.0 && __instance.GetComponent<Battery>() == null)
				{
					Descriptor descriptor = new Descriptor();
					descriptor.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ENERGYGENERATED, GameUtil.GetFormattedWattage(def.GeneratorWattageRating)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ENERGYGENERATED, GameUtil.GetFormattedWattage(def.GeneratorWattageRating)));
					descriptorList.Add(descriptor);
				}
				if (def.ExhaustKilowattsWhenActive > 0.0 || def.SelfHeatKilowattsWhenActive > 0.0)
				{
					Descriptor descriptor = new Descriptor();
					descriptor.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATGENERATED, GameUtil.GetFormattedWattage((float)(5.0 * (def.ExhaustKilowattsWhenActive + (double)def.SelfHeatKilowattsWhenActive)))), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED, GameUtil.GetFormattedJoules((float)(5.0 * (def.ExhaustKilowattsWhenActive + (double)def.SelfHeatKilowattsWhenActive)), "F1")));
					descriptorList.Add(descriptor);
				}
				__result = descriptorList;
			}
		}

		[HarmonyPatch(typeof(LiquidCooledFan), "GetDescriptors")]
		public class LiquidCooledFanPatch
		{
			private static void Postfix(LiquidCooledFan __instance, ref List<Descriptor> __result)
			{
				List<Descriptor> descriptorList = new List<Descriptor>();
				Descriptor descriptor = new Descriptor();
				descriptor.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATCONSUMED, GameUtil.GetFormattedWattage(__instance.coolingKilowatts)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATCONSUMED, GameUtil.GetFormattedJoules(__instance.coolingKilowatts, "F1")));
				descriptorList.Add(descriptor);
				__result = descriptorList;
			}
		}

		[HarmonyPatch(typeof(StructureTemperatureComponents), "InitializeStatusItem")]
		public class StructureTemperatureComponentsPatch
		{
			private static void Postfix(StructureTemperatureComponents __instance)
			{
				var operatingEnergyStatusItem = Traverse.Create((object)__instance).Field("operatingEnergyStatusItem").GetValue<StatusItem>();
				var handleInstanceMap = Traverse.Create((object)__instance).Field("handleInstanceMap").GetValue<Dictionary<int, HandleVector<int>.Handle>>();
				if (operatingEnergyStatusItem == null || handleInstanceMap == null)
					return;

				operatingEnergyStatusItem.resolveStringCallback = (Func<string, object, string>)((str, ev_data) =>
				{
					int index = (int)ev_data;
					StructureTemperatureData data = __instance.GetData(handleInstanceMap[index]);
					if (str != (string)BUILDING.STATUSITEMS.OPERATINGENERGY.TOOLTIP)
					{
						try
						{
							str = string.Format(str, GameUtil.GetFormattedWattage((float)(data.TotalEnergyProducedKW * 1000.0 * 0.00499999988824129)));
						}
						catch (Exception ex)
						{
							Debug.LogWarning(ex, null);
							Debug.LogWarning(BUILDING.STATUSITEMS.OPERATINGENERGY.TOOLTIP, null);
							Debug.LogWarning(str, null);
						}
					}
					else
					{
						string empty = string.Empty;
						foreach (StructureTemperatureData.EnergySource energySource in data.energySourcesKW)
							empty += string.Format(BUILDING.STATUSITEMS.OPERATINGENERGY.LINEITEM, energySource.source, GameUtil.GetFormattedWattage((float)(energySource.value * 1000.0 * 0.00499999988824129)));
						str = string.Format((LocString)"This building is producing {0} of energy\n\nSources:\n{1}", GameUtil.GetFormattedWattage((float)(data.TotalEnergyProducedKW * 1000.0 * 0.00499999988824129)), empty);
					}
					return str;
				});
			}
		}

		[HarmonyPatch(typeof(GameUtil), "GetSHCSuffix")]
		public class GetSHCSuffixPatch
		{
			private static bool Prefix(ref string __result)
			{
				__result = string.Format("(J/g)/{0}", (object)GameUtil.GetTemperatureUnitSuffix());
				return false;
			}
		}

		[HarmonyPatch(typeof(GameUtil), "GetFormattedSHC")]
		public class GetFormattedSHCPatch
		{
			private static bool Prefix(float shc, ref string __result)
			{
				shc = GameUtil.GetDisplaySHC(shc);
				__result = string.Format("{0} (J/g)/{1}", (object)shc.ToString("0.000"), (object)GameUtil.GetTemperatureUnitSuffix());
				return false;
			}
		}

		[HarmonyPatch(typeof(GameUtil), "GetThermalConductivitySuffix")]
		public class GetThermalConductivitySuffixPatch
		{
			private static bool Prefix(ref string __result)
			{
				__result = string.Format("(W/m)/{0}", (object)GameUtil.GetTemperatureUnitSuffix());
				return false;
			}
		}

		[HarmonyPatch(typeof(GameUtil), "GetFormattedThermalConductivity")]
		public class GetFormattedThermalConductivityPatch
		{
			private static bool Prefix(float tc, ref string __result)
			{
				tc = GameUtil.GetDisplayThermalConductivity(tc);
				__result = string.Format("{0} (W/m)/{1}", (object)tc.ToString("0.000"), (object)GameUtil.GetTemperatureUnitSuffix());
				return false;
			}
		}
	}
}