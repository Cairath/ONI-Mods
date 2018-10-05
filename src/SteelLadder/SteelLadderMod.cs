using System;
using System.Collections.Generic;
using Harmony;

namespace SteelLadder
{
	public class SteelLadderMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class SteelLadderBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.STEELLADDER.NAME", "Steel Ladder");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.STEELLADDER.DESC", "Durable steel ladder with plastic handles, combining usefulness and aesthetics.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.STEELLADDER.EFFECT", "Increases Duplicant climbing speed.");

				List<string> buldings = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[0].data) { SteelLadderConfig.ID };
				TUNING.BUILDINGS.PLANORDER[0].data = buldings.ToArray();
			}

			private static void Postfix()
			{
				object obj = Activator.CreateInstance(typeof(SteelLadderConfig));
				BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class SteelLadderDbPatch
		{
			private static void Prefix()
			{
				List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { SteelLadderConfig.ID };
				Database.Techs.TECH_GROUPING["Luxury"] = ls.ToArray();
			}
		}
	}
}