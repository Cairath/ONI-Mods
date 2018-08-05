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

		[HarmonyPatch(typeof(SeasonManager))]
		public static class ElementLoaderLoadPatch2
		{
			static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
			{
				int foundIndex = 0;
				var codes = new List<CodeInstruction>(instr);
				Debug.Log("kupa1 instr count:" + codes.Count);

				int idxDefaultLenght = 0;
				int idxMeteorLenght = 0;
				int idxSecondsBombardmentOff = 0;
				int idxSecondsBombardmentOn = 0;
				int idxSecondsBetweenBombardments = 0;

				for (int j = 0; j< codes.Count; j++)
				{
					Debug.Log(codes[j].opcode.Name + " " + codes[j].operand);
					if (codes[j].opcode == OpCodes.Ldc_I4_4)
					{
						idxDefaultLenght = j;
						break;
					}
				}

				codes[idxDefaultLenght].opcode = OpCodes.Ldc_I4_1;

				return codes.AsEnumerable();
			}
		}
	}
}
