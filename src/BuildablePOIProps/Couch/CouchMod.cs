using System;
using System.Collections.Generic;
using Harmony;

namespace BuildablePOIProps.Couch
{
	public class CouchMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class CouchBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.COUCH.NAME", "Couch");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.COUCH.DESC", "Comfier than it looks!");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.COUCH.EFFECT", "Perfect for some relaxation.");

				List<string> buldings = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[8].data) { CouchConfig.ID };
				TUNING.BUILDINGS.PLANORDER[8].data = buldings.ToArray();

				TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(CouchConfig.ID);
			}

			private static void Postfix()
			{
				object obj = Activator.CreateInstance(typeof(CouchConfig));
				BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class CouchDbPatch
		{
			private static void Prefix()
			{
				List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { CouchConfig.ID };
				Database.Techs.TECH_GROUPING["Luxury"] = ls.ToArray();
			}
		}
	}
}