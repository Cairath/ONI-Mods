using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Harmony;

namespace MeteorFrequency
{
    public class MeteorFrequencyMod
    {
	    [HarmonyPatch(typeof(SeasonManager))]
	    public static class MeteorFrequencySeasonManagerPatch
		{
		    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		    {
			    var codes = new List<CodeInstruction>(instructions);
			    for (int i = 0; i < codes.Count; i++)
			    {
				    if (codes[i].opcode == OpCodes.Ldc_I4_4)
				    {
					    codes[i].opcode = OpCodes.Ldc_I4;
					    codes[i].operand = 20;

				    }
			    }

			    return codes.AsEnumerable();
		    }
	    }
	}
}
