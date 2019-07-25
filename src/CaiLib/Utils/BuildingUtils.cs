using System.Collections.Generic;
using TUNING;

namespace CaiLib.Utils
{
	public class BuildingUtils
	{
		public static void AddBuildingToPlanScreen(HashedString category, string buildingId, string addAfterBuildingId = null)
		{
			var index = BUILDINGS.PLANORDER.FindIndex(x => x.category == category);

			if (index == -1)
				return;

			var planOrderList = BUILDINGS.PLANORDER[index].data as IList<string>;
			if (planOrderList == null)
			{
				Logger.Logger.Log("CaiLibUtils", $"Could not add {buildingId} to the building menu.");
				return;
			}
			
			var neighborIdx = planOrderList.IndexOf(addAfterBuildingId);

			if (neighborIdx != -1)
				planOrderList.Insert(neighborIdx + 1, buildingId);
			else
				planOrderList.Add(buildingId);
		}

		public static void AddBuildingToTechnology(string tech, string buildingId)
		{
			var techList = new List<string>(Database.Techs.TECH_GROUPING[tech]) { buildingId };
			Database.Techs.TECH_GROUPING[tech] = techList.ToArray();
		}

		public static void AddBuildingStrings(string buildingId, string name, string description, string effect)
		{
			Strings.Add($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.NAME", name);
			Strings.Add($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.DESC", description);
			Strings.Add($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.EFFECT", effect);
		}
	}
}