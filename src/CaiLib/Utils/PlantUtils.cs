using TUNING;

namespace CaiLib.Utils
{
	public class PlantUtils
	{
		public static void AddCropType(string cropId, float domesticatedGrowthTimeInCycles, int producedPerHarvest)
		{
			CROPS.CROP_TYPES.Add(new Crop.CropVal(cropId, domesticatedGrowthTimeInCycles * 600, producedPerHarvest));
		}
	}
}