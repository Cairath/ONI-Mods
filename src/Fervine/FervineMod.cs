using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Harmony;
using KSerialization;
using TUNING;

namespace Fervine
{
	public class FervineMod
	{
		public static SpaceDestinationType MiniSun = new SpaceDestinationType(nameof(MiniSun), Db.Get().Root, "Mini Sun", "CAUTION: HOT", 16, "sun", new Dictionary<SimHashes, MathUtil.MinMax>()
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

		[HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
		public class FervineEntityConfigManagerPatch
		{

			private static void Prefix()
			{
				Strings.Add("STRINGS.CREATURES.SPECIES.SEEDS.HEATBULB.NAME", FervineConfig.SeedName);
				Strings.Add("STRINGS.CREATURES.SPECIES.SEEDS.HEATBULB.DESC", FervineConfig.SeedDesc);
			}
		}

		[HarmonyPatch(typeof(SpacecraftManager), "OnPrefabInit")]
		public static class SpaceManagerPatch
		{
			public static void Postfix(ref SpacecraftManager __instance)
			{
				if (__instance.destinations == null)
					return;
				
				Db.Get().SpaceDestinationTypes.Add(MiniSun);

				var destination = new SpaceDestination(__instance.destinations.Count, MiniSun.Id, 6);
				__instance.destinations.Add(destination);
				destination.startAnalyzed = true;
				SpacecraftManager.instance.EarnDestinationAnalysisPoints(destination.id, 10000f);
			}
		}

		[HarmonyPatch(typeof(SpacecraftManager), "OnSpawn")]
		public static class SpaceManagerSpawnPatch
		{
			public static void Postfix(ref SpacecraftManager __instance)
			{
				if (__instance.destinationsGenerated && __instance.destinations != null &&
				    __instance.destinations.All(d => d.type != nameof(MiniSun)))
				{
					Db.Get().SpaceDestinationTypes.Add(MiniSun);

					var destination = new SpaceDestination(__instance.destinations.Count, MiniSun.Id, 6);
					__instance.destinations.Add(destination);
					destination.startAnalyzed = true;
					destination.startingOrbitPercentage = 0.55f;
					SpacecraftManager.instance.EarnDestinationAnalysisPoints(destination.id, 10000f);
				}
			}
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