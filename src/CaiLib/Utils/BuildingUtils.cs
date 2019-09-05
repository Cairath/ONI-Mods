using System.Collections.Generic;
using TUNING;

namespace CaiLib.Utils
{
	public static class BuildingUtils
	{
		public static void AddBuildingToPlanScreen(HashedString category, string buildingId, string addAfterBuildingId = null)
		{
			var index = BUILDINGS.PLANORDER.FindIndex(x => x.category == category);

			if (index == -1)
				return;

			var planOrderList = BUILDINGS.PLANORDER[index].data as IList<string>;
			if (planOrderList == null)
			{
				Logger.Logger.Log($"Could not add {buildingId} to the building menu.");
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
    }
}