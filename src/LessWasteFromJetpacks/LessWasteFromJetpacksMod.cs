using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Harmony;

namespace LessWasteFromJetpacks
{
    public class LessWasteFromJetpacksMod
    {
	    [HarmonyPatch(typeof(JetSuitMonitor), "Emit")]
	    public static class MeteorFrequencySeasonManagerPatch
	    {
		    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		    {
				var codes = new List<CodeInstruction>(instructions);
			    for (int i = 0; i < codes.Count; i++)
			    {
					if (codes[i].opcode == OpCodes.Ldc_R4 && ((float)codes[i].operand) == 3f)
				    {
					    codes[i].operand = 0f;
					    break;
				    }
			    }

			    return codes.AsEnumerable();
		    }
	    }
	}
}
