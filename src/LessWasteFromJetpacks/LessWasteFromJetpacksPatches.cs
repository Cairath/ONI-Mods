using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Harmony;

namespace LessWasteFromJetpacks
{
	public class LessWasteFromJetpacksPatches
	{
		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				CaiLib.Logger.LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(JetSuitMonitor))]
		[HarmonyPatch("Emit")]
		public static class JetSuitMonitor_Emit_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
			{
				var codes = new List<CodeInstruction>(instructions);
				for (int i = 0; i < codes.Count; i++)
				{
					if (codes[i].opcode == OpCodes.Ldc_R4 && (float)codes[i].operand == 3f)
					{
						codes[i].operand = 1f;
						break;
					}
				}

				return codes.AsEnumerable();
			}
		}
	}
}
