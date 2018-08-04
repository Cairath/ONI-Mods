using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using Klei;
using STRINGS;
using UnityEngine;

namespace PWaterToWaterHeatCapacity
{
	public class PWaterToWaterHeatCapacityMod
	{
		[HarmonyPatch(typeof(ElementLoader), "Load")]
		public static class ElementLoaderLoadPatch
		{
			public static void Prefix(ref ElementLoader.LiquidEntry[] liquid_entries)
			{
				MethodInfo method =
					typeof(ElementLoader).GetMethod("SetupElementsTable", BindingFlags.NonPublic | BindingFlags.Static);

				if (method == null || liquid_entries == null)
				{
					Debug.Log("[MOD] PWaterToWaterHeatCapacity: could not initialize elements table, abort");
					return;
				}

				method.Invoke(null, null);

				foreach (var e in liquid_entries)
				{
					Element elementByHash = ElementLoader.FindElementByHash(e.elementId);
					if (elementByHash.id == SimHashes.DirtyWater)
					{
						e.specificHeatCapacity = 4.179f;
					}
				}
			}
		}	
	}
}
