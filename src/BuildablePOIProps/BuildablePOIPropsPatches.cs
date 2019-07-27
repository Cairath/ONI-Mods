using Harmony;

namespace BuildablePOIProps
{
	public static class BuildablePOIPropsPatches
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