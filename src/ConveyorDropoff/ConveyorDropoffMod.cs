using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace ConveyorDropoff
{
    public class ConveyorDropoffMod
    {
	    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	    public class SolidConduitFilterBuildingsPatch
	    {
		    private static void Prefix()
		    {
			    Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ConveyorDropoffConfig.ID.ToUpper()}.NAME", "Conveyor Dropoff Point");
			    Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ConveyorDropoffConfig.ID.ToUpper()}.DESC", "A garbage collector!");
			    Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ConveyorDropoffConfig.ID.ToUpper()}.EFFECT", "A place for the Auto-Sweepers to drop stuff on the ground.");

			    List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => po.category == PlanScreen.PlanCategory.Conveyance).data;
			    category.Add(ConveyorDropoffConfig.ID);
		    }
	    }

	    [HarmonyPatch(typeof(Db), "Initialize")]
	    public class SolidConduitFilterDbPatch
	    {
		    private static void Prefix()
		    {
			    List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["SolidTransport"]) { ConveyorDropoffConfig.ID };
			    Database.Techs.TECH_GROUPING["SolidTransport"] = ls.ToArray();
		    }
	    }   
	}
}
