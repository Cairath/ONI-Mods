using System;
using System.Linq;
using Harmony;

namespace CaiLib.Utils
{
	public class CarePackagesUtils
	{
		/*
		 * To be used in a POSTFIX patch:
		 * [HarmonyPatch(typeof(Immigration))]
		 * [HarmonyPatch("ConfigureCarePackages")]
		 */
		public static void AddCarePackage(ref Immigration immigration, string objectId, float amount, Func<bool> requirement = null)
		{
			var field = Traverse.Create(immigration).Field("carePackages");
			var list = field.GetValue<CarePackageInfo[]>().ToList();

			list.Add(new CarePackageInfo(objectId, amount, requirement));

			field.SetValue(list.ToArray());
		}

		public static bool CycleCondition(int cycle)
		{
			return GameClock.Instance.GetCycle() >= cycle;
		}

		public static bool DiscoveredCondition(Tag tag)
		{
			return WorldInventory.Instance.IsDiscovered(tag);
		}
	}
}