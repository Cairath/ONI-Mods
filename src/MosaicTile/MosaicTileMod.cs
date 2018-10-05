using System;
using System.Collections.Generic;
using Harmony;

namespace MosaicTiles
{
    public class MosaicTileMod
    {
	    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	    public class MosaicTileBuildingsPatch
	    {
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.MOSAICTILE.NAME", "Mosaic Tile");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.MOSAICTILE.DESC", String.Empty);
				Strings.Add("STRINGS.BUILDINGS.PREFABS.MOSAICTILE.EFFECT", "Used as floor and wall tile to build rooms.\n\nSignificantly increases Duplicant runspeed.");
				
				List<string> buldings = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[0].data) { MosaicTileConfig.ID };
				TUNING.BUILDINGS.PLANORDER[0].data = buldings.ToArray();
			}

		    private static void Postfix()
		    {
			    object obj = Activator.CreateInstance(typeof(MosaicTileConfig));
			    BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
		    }
		}

	    [HarmonyPatch(typeof(Db), "Initialize")]
	    public class MosaicTileDbPatch
		{
		    private static void Prefix()
		    {
				List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { MosaicTileConfig.ID };
			    Database.Techs.TECH_GROUPING["Luxury"] = ls.ToArray();
		    }
	    }
	}
}
