using Database;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using TUNING;

namespace DecorLights
{
	public class DecorLightsMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class DecorLightsBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.LAVALAMP.NAME", "Lava Lamp");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.LAVALAMP.DESC", "More light, more heat.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.LAVALAMP.EFFECT", STRINGS.BUILDINGS.PREFABS.CEILINGLIGHT.DESC);

				Strings.Add("STRINGS.BUILDINGS.PREFABS.SALTLAMP.NAME", "Salt Lamp");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.SALTLAMP.DESC", "Fake salt. Not edible.Do not lick.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.SALTLAMP.EFFECT", STRINGS.BUILDINGS.PREFABS.CEILINGLIGHT.DESC);

				Strings.Add("STRINGS.BUILDINGS.PREFABS.CEILINGLAMP.NAME", "Ceiling Lamp");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.CEILINGLAMP.DESC", "Like normal ceiling light, but prettier.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.CEILINGLAMP.EFFECT", STRINGS.BUILDINGS.PREFABS.CEILINGLIGHT.DESC);

				Strings.Add("STRINGS.BUILDINGS.PREFABS.WALLLAMP.NAME", "Wall Lamp");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.WALLLAMP.DESC", "Light: now in Wall Edition. Ceiling hook not included.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.WALLLAMP.EFFECT", STRINGS.BUILDINGS.PREFABS.CEILINGLIGHT.DESC);

				List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => po.category == PlanScreen.PlanCategory.Furniture).data;
				category.Add(LavaLampConfig.ID);
				category.Add(SaltLampConfig.ID);
				category.Add(CeilingLampConfig.ID);
				category.Add(WallLampConfig.ID);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class DecorLightsDbPatch
		{
			private static void Prefix()
			{
				List<string> ls = new List<string>(Techs.TECH_GROUPING["Luxury"]) { LavaLampConfig.ID, SaltLampConfig.ID, CeilingLampConfig.ID, WallLampConfig.ID};
				Techs.TECH_GROUPING["Luxury"] = ls.ToArray();
			}
		}
	}
}