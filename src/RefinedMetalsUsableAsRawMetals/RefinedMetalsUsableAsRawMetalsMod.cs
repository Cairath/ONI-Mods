using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;

namespace RefinedMetalsUsableAsRawMetals
{
	public class RefinedMetalsUsableAsRawMetalsMod
	{
		[HarmonyPatch(typeof(ElementLoader), "Load")]
		public static class ElementLoaderLoadPatch
		{
			public static void Prefix(ref ElementLoader.SolidEntry[] solid_entries)
			{
				MethodInfo method =
					typeof(ElementLoader).GetMethod("SetupElementsTable", BindingFlags.NonPublic | BindingFlags.Static);

				if (method == null || solid_entries == null)
				{
					Debug.Log("[MOD] RefinedMetalsUsableAsRawMetals: could not initialize elements table, abort");
					return;
				}

				method.Invoke(null, null);

				foreach (var e in solid_entries)
				{
					if (e.materialCategory.Contains("RefinedMetal")) 
					{
						e.tags += " | RefinedMetal | Metal";
					} else if (e.tags.Contains("RefinedMetal"))
					{
						e.tags += " | Metal";
					}
				}
			}
		}
	}
}
