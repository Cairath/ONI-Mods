using Harmony;

namespace RanchingRebalanced
{
	public static class RanchingRebalancedPatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				CaiLib.Logger.Logger.LogInit(ModInfo.Name, ModInfo.Version);
			}
		}
	}
}