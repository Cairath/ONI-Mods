using System;
using Harmony;
using UnityEngine;
using System.Collections.Generic;

namespace PalmeraTree
{
	public class PalmeraTreeMod
	{
		[HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
		public class PalmeraTreeEntityConfigManagerPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.TRELLIS.NAME", "Trellis");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.TRELLIS.DESC", "Used to plant trees.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.TRELLIS.EFFECT", "For when you want to grow your very own tree.");

				List<string> farm =
					new List<string>((string[])TUNING.BUILDINGS.PLANORDER[3].data) { TrellisConfig.ID };
				TUNING.BUILDINGS.PLANORDER[3].data = farm.ToArray();
				TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(TrellisConfig.ID);


				TUNING.CROPS.CROP_TYPES.Add(new Crop.CropVal(PalmeraBerryConfig.ID, 800f, 1, true));
			}

			private static void Postfix()
			{
				object berry = Activator.CreateInstance(typeof(PalmeraBerryConfig));
				EntityConfigManager.Instance.RegisterEntity(berry as IEntityConfig);

				object tree = Activator.CreateInstance(typeof(PalmeraTreeConfig));
				EntityConfigManager.Instance.RegisterEntity(tree as IEntityConfig);

				object trellis = Activator.CreateInstance(typeof(TrellisConfig));
				BuildingConfigManager.Instance.RegisterBuilding(trellis as IBuildingConfig);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class PalmeraTreeDbPatch
		{
			private static void Prefix()
			{
				List<string> tech = new List<string>(Database.Techs.TECH_GROUPING["FarmingTech"]) { TrellisConfig.ID };
				Database.Techs.TECH_GROUPING["FarmingTech"] = tech.ToArray();
			}
		}

		[HarmonyPatch(typeof(KSerialization.Manager), "GetType", new Type[] { typeof(string) })]
		public static class PalmeraTreeEntitySerializationPatch
		{
			[HarmonyPostfix]
			public static void GetType(string type_name, ref Type __result)
			{
				if (type_name == "PalmeraTree.PalmeraTree")
				{
					__result = typeof(PalmeraTree);
				}
			}
		}
	}
}