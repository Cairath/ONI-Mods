using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace BiggerCameraZoomOut
{
	public class BiggerCameraZoomOutMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}