using System;
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
				__instance.destinations.Add(new MiniSun(21, 4, 0.6f, ROCKETRY.DESTINATION_THRUST_COSTS.HIGH));
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