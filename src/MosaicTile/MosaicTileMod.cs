using System;
using System.Collections.Generic;
using System.Linq;
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

				List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => po.category == PlanScreen.PlanCategory.Base).data;
				category.Add(MosaicTileConfig.ID);
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
