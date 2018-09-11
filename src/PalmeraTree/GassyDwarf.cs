using System.Collections.Generic;
using STRINGS;

namespace PalmeraTree
{
	public class GassyDwarf : DwarfPlanet
	{
		public static LocString Name = "Gassy Dwarf";
		public static LocString Description = "A hot, gassy dwarf. Under many layers of gas there is some hot soil, home to the native Palmera Tree.";

		public GassyDwarf(int id, int distance, float startPosition, int thrustCost)
			: base(id, distance, startPosition, thrustCost)
		{
			this.name = Name;
			this.typeName = Name;
			this.description = Description;
			this.spriteName = "gasGiant";
			this.elementTable = new Dictionary<SimHashes, Tuple<float, float>>()
			{
				{
					SimHashes.ChlorineGas,
					new Tuple<float, float>(200f, 300f)
				},
				{
					SimHashes.Hydrogen,
					new Tuple<float, float>(50f, 100f)
				},
				{
					SimHashes.Dirt,
					new Tuple<float, float>(100f, 200f)
				}
			};
			this.recoverableEntities = new Dictionary<string, int>()
			{
				{
					PalmeraTreeConfig.SEED_ID,
					1
				},
			};
			this.GenerateSurfaceElements();
		}
	}
}