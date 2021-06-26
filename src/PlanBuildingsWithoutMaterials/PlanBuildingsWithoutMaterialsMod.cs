using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace PlanBuildingsWithoutMaterials
{
	public class PlanBuildingsWithoutMaterialsMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}