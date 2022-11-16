namespace CaiLib.Utils
{
	public static class BuildingUtils
	{
		public static void AddBuildingToPlanScreen(HashedString category, string buildingId, string subCategory = "uncategorized", string addAfterBuildingId = null)
		{
			ModUtil.AddBuildingToPlanScreen(category, buildingId, subCategory, addAfterBuildingId);
		}

		public static void AddBuildingToTechnology(string techId, string buildingId)
		{
			Db.Get().Techs.Get(techId).unlockedItemIDs.Add(buildingId);
		}
    }
}