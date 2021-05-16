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

			if (!(BUILDINGS.PLANORDER[index].data is IList<string> planOrderList))
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

		public static void AddBuildingToTechnology(string techId, string buildingId)
		{
			Db.Get().Techs.Get(techId).unlockedItemIDs.Add(buildingId);
		}
    }
}