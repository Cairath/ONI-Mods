using System;
using System.Collections.Generic;
using Harmony;

namespace TableWithChairs
{
	public class TableWithChairsMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class TableWithChairsBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.TABLEWITHCHAIRS.NAME", "Table");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.TABLEWITHCHAIRS.DESC", "A table and some chairs.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.TABLEWITHCHAIRS.EFFECT", "Perfect for a break.");

				List<string> buldings = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[8].data) { TableWithChairsConfig.ID };
				TUNING.BUILDINGS.PLANORDER[8].data = buldings.ToArray();

				TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(TableWithChairsConfig.ID);
			}

			private static void Postfix()
			{
				object obj = Activator.CreateInstance(typeof(TableWithChairsConfig));
				BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class TableWithChairsDbPatch
		{
			private static void Prefix()
			{
				List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { TableWithChairsConfig.ID };
				Database.Techs.TECH_GROUPING["Luxury"] = ls.ToArray();
			}
		}
	}
}