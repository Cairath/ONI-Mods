using System.Collections.Generic;
using TUNING;
using static CaiLib.Logger.Logger;

namespace CaiLib.Utils
{
	public static class BuildingUtils
	{
		public static void AddBuildingToPlanScreen(HashedString category, string buildingId, string addAfterBuildingId = null)
		{
			var index = BUILDINGS.PLANORDER.FindIndex(x => x.category == category);

			if ( index == -1 )
			{
				Log($"Invalid building category {category}");
				return;
			}

			var planOrderList = BUILDINGS.PLANORDER[index].data as IList<string>;
			if (planOrderList == null)
			{
				Log($"Could not add {buildingId} to the building menu.");
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
			if ( !Database.Techs.TECH_GROUPING.ContainsKey( tech ) )
			{
				Log( $"[WARNING] Technology {tech} does not exist!" );
			}
			else
			{
				var techList = new List<string>(Database.Techs.TECH_GROUPING[tech]) { buildingId };
				Database.Techs.TECH_GROUPING[tech] = techList.ToArray();
			}
		}
    }
}