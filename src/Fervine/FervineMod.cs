using System;
using System.Collections.Generic;
using Database;
using Harmony;
using KSerialization;
using TUNING;

namespace Fervine
{
	public class FervineMod
	{
		[HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
		public class FervineEntityConfigManagerPatch
		{

			private static void Prefix()
			{
				Strings.Add("STRINGS.CREATURES.SPECIES.SEEDS.HEATBULB.NAME", FervineConfig.SeedName);
				Strings.Add("STRINGS.CREATURES.SPECIES.SEEDS.HEATBULB.DESC", FervineConfig.SeedDesc);			
			}

			private static void Postfix()
			{
				object heatbulb = Activator.CreateInstance(typeof(FervineConfig));
				EntityConfigManager.Instance.RegisterEntity(heatbulb as IEntityConfig);
			}
		}

		[HarmonyPatch(typeof(SpacecraftManager), "OnPrefabInit")]
		public static class SpaceManagerPatch
		{
			public static void Postfix(ref SpacecraftManager __instance)
			{
				SpaceDestinationType MiniSun;
				MiniSun = new SpaceDestinationType(nameof(MiniSun), null, "Mini Sun", "CAUTION: HOT", 16, "sun", new Dictionary<SimHashes, MathUtil.MinMax>()
				{
					{
						SimHashes.Hydrogen,
						new MathUtil.MinMax(98f, 99f)
					},
					{
						SimHashes.GoldAmalgam,
						new MathUtil.MinMax(1f, 2f)
					}
				}, new Dictionary<string, int>
				{
					{
						FervineConfig.SEED_ID,
						1
					},
					{
						LightBugConfig.EGG_ID,
						1
					},
				});

				__instance.destinations.Add(new SpaceDestination(__instance.destinations.Count, MiniSun.Id, 15));
			}
		}

		[HarmonyPatch(typeof(Manager), "GetType", new[] { typeof(string) })]
		public static class FervineEntitySerializationPatch
		{
			public static void Postfix(string type_name, ref Type __result)
			{
				if (type_name == "Fervine.Fervine")
				{
					__result = typeof(Fervine);
				}
			}
		}
	}
}