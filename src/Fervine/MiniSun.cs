using System.Collections.Generic;

namespace Fervine
{
	public class MiniSun : SpaceDestination
	{
		public static LocString Name = "Mini Sun";
		public static LocString Description = "CAUTION: HOT";

		public MiniSun(int id, int distance, float startPosition, int thrustCost)
			: base(id, distance, startPosition, thrustCost)
		{
			name = Name;
			typeName = Name;
			description = Description;
			spriteName = "sun";
			elementTable = new Dictionary<SimHashes, Tuple<float, float>>()
			{
				{
					SimHashes.Hydrogen,
					new Tuple<float, float>(98f, 99f)
				},
				{
					SimHashes.GoldAmalgam,
					new Tuple<float, float>(1f, 2f)
				}
			};
			recoverableEntities = new Dictionary<string, int>()
			{
				{
					FervineConfig.SEED_ID,
					1
				},
				{
					LightBugConfig.EGG_ID,
					1
				},
			};

			GenerateSurfaceElements();
		}
	}
}