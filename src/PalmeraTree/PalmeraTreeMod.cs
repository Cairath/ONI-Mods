using System;
using System.Collections.Generic;
using Database;
using Harmony;
using KSerialization;
using STRINGS;
using TUNING;
using BUILDINGS = TUNING.BUILDINGS;

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

				Strings.Add("STRINGS.CREATURES.SPECIES.SEEDS.PALMERATREEPLANT.NAME", PalmeraTreeConfig.SeedName);
				Strings.Add("STRINGS.CREATURES.SPECIES.SEEDS.PALMERATREEPLANT.DESC", PalmeraTreeConfig.SeedDesc);

				Strings.Add("STRINGS.CREATURES.SPECIES.PALMERATREEPLANT.NAME", PalmeraTreeConfig.Name);
				Strings.Add("STRINGS.CREATURES.SPECIES.PALMERATREEPLANT.DESC", PalmeraTreeConfig.Desc);
				Strings.Add("STRINGS.CREATURES.SPECIES.PALMERATREEPLANT.DOMESTICATEDDESC", PalmeraTreeConfig.DomesticatedDesc);

				Strings.Add("STRINGS.ITEMS.FOOD." + SteamedPalmeraBerryConfig.ID.ToUpper() + ".NAME",
					SteamedPalmeraBerryConfig.NameStr);
				Strings.Add("STRINGS.ITEMS.FOOD." + SteamedPalmeraBerryConfig.ID.ToUpper() + ".DESC",
					SteamedPalmeraBerryConfig.Desc);
				Strings.Add("STRINGS.ITEMS.FOOD." + SteamedPalmeraBerryConfig.ID.ToUpper() + ".RECIPEDESC",
					SteamedPalmeraBerryConfig.RecipeDesc);

				Strings.Add("STRINGS.ITEMS.FOOD." + PalmeraBerryConfig.ID.ToUpper() + ".NAME", PalmeraBerryConfig.NameStr);
				Strings.Add("STRINGS.ITEMS.FOOD." + PalmeraBerryConfig.ID.ToUpper() + ".DESC", PalmeraBerryConfig.Desc);

				List<string> farm =
					new List<string>((string[])BUILDINGS.PLANORDER[3].data) { TrellisConfig.ID };
				BUILDINGS.PLANORDER[3].data = farm.ToArray();

				CROPS.CROP_TYPES.Add(new Crop.CropVal(PalmeraBerryConfig.ID, 12000f, 10));
			}

			private static void Postfix()
			{
				object berry = Activator.CreateInstance(typeof(PalmeraBerryConfig));
				EntityConfigManager.Instance.RegisterEntity(berry as IEntityConfig);

				object cookedBerry = Activator.CreateInstance(typeof(SteamedPalmeraBerryConfig));
				EntityConfigManager.Instance.RegisterEntity(cookedBerry as IEntityConfig);

				object tree = Activator.CreateInstance(typeof(PalmeraTreeConfig));
				EntityConfigManager.Instance.RegisterEntity(tree as IEntityConfig);

				object trellis = Activator.CreateInstance(typeof(TrellisConfig));
				BuildingConfigManager.Instance.RegisterBuilding(trellis as IBuildingConfig);
			}
		}

		[HarmonyPatch(typeof(BuildingLoader), "Add2DComponents")]
		public class Add2DComponentsPatch
		{
			private static void Prefix(ref string initialAnimState, BuildingDef def)
			{
				if (initialAnimState == "place" && def.Name == TrellisConfig.ID) initialAnimState = "place_1";
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

		[HarmonyPatch(typeof(Manager), "GetType", new[] { typeof(string) })]
		public static class PalmeraTreeEntitySerializationPatch
		{
			public static void Postfix(string type_name, ref Type __result)
			{
				if (type_name == "PalmeraTree.PalmeraTree")
				{
					__result = typeof(PalmeraTree);
				}
			}
		}

		[HarmonyPatch(typeof(SpacecraftManager), "OnPrefabInit")]
		public static class SpaceManagerPatch
		{
			public static void Postfix(ref SpacecraftManager __instance)
			{
				SpaceDestinationType GassyDwarf;
				GassyDwarf = new SpaceDestinationType(nameof(GassyDwarf), null, "Gassy Dwarf", "A hot, gassy dwarf. Under many layers of gas there is some hot soil, home to the native Palmera Tree.", 16, "gasGiant", new Dictionary<SimHashes, MathUtil.MinMax>()
				{
					{
						SimHashes.ChlorineGas,
						new MathUtil.MinMax(200f, 300f)
					},
					{
						SimHashes.Hydrogen,
						new MathUtil.MinMax(50f, 100f)
					},
					{
						SimHashes.Dirt,
						new MathUtil.MinMax(100f, 200f)
					}
				}, new Dictionary<string, int>
				{
					{
						PalmeraTreeConfig.SEED_ID,
						1
					}
				});

				__instance.destinations.Add(new SpaceDestination(__instance.destinations.Count, GassyDwarf.Id, 20));
			}
		}
	}
}