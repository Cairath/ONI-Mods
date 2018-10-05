using System;
using System.Collections.Generic;
using Harmony;

namespace BuildablePOIProps.Chair
{
	public class ChairMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class ChairBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.CHAIRLEFT.NAME", "Chair (left)");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.CHAIRLEFT.DESC", "A comfy chair.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.CHAIRLEFT.EFFECT", "So comfy!");

				Strings.Add("STRINGS.BUILDINGS.PREFABS.CHAIRRIGHT.NAME", "Chair (right)");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.CHAIRRIGHT.DESC", "A comfy chair.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.CHAIRRIGHT.EFFECT", "So comfy!");

				List<string> buldings = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[8].data) { ChairLeftConfig.ID, ChairRightConfig.ID };
				TUNING.BUILDINGS.PLANORDER[8].data = buldings.ToArray();
			}

			private static void Postfix()
			{
				object obj = Activator.CreateInstance(typeof(ChairLeftConfig));
				BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);

				obj = Activator.CreateInstance(typeof(ChairRightConfig));
				BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class ChairDbPatch
		{
			private static void Prefix()
			{
				List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { ChairLeftConfig.ID, ChairRightConfig.ID };
				Database.Techs.TECH_GROUPING["Luxury"] = ls.ToArray();
			}
		}
	}
}