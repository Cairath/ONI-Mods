using System;
using System.Collections.Generic;
using Harmony;

namespace ComputerDesk
{
	public class ComputerDeskMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class ComputerDeskBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.COMPUTERDESK.NAME", "Computer desk");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.COMPUTERDESK.DESC", "An intact office desk, decorated with several personal belongings and a barely functioning computer.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.COMPUTERDESK.EFFECT", "Does it work? Who knows.");
				
				List<string> buldings = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[8].data) { ComputerDeskConfig.ID };
				TUNING.BUILDINGS.PLANORDER[8].data = buldings.ToArray();
				TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(ComputerDeskConfig.ID);
			}

			private static void Postfix()
			{
				object obj = Activator.CreateInstance(typeof(ComputerDeskConfig));
				BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class ComputerDeskDbPatch
		{
			private static void Prefix()
			{
				List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { ComputerDeskConfig.ID };
				Database.Techs.TECH_GROUPING["Luxury"] = ls.ToArray();
			}
		}
	}
}