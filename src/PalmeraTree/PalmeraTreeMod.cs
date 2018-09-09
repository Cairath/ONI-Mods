using Harmony;
using KSerialization;
using TUNING;

namespace PalmeraTree
{
	public class PalmeraTreeMod
	{
		[HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
		public class PalmeraTreeEntityConfigManagerPatch
		{
			private static void Prefix()
			{
				CROPS.CROP_TYPES.Add(new Crop.CropVal(PalmeraBerryConfig.ID, 800f));
			}

			private static void Postfix()
			{
				object berry = Activator.CreateInstance(typeof(PalmeraBerryConfig));
				EntityConfigManager.Instance.RegisterEntity(berry as IEntityConfig);

				object tree = Activator.CreateInstance(typeof(PalmeraTreeConfig));
				EntityConfigManager.Instance.RegisterEntity(tree as IEntityConfig);
			}
		}

		[HarmonyPatch(typeof(Manager), "GetType", new[] { typeof(string) })]
		public static class PalmeraTreeEntitySerializationPatch
		{
			public static void Postfix(string type_name, ref Type __result)
			{
				if (type_name == "RanchingRebalanced.PalmeraTree.PalmeraTree")
				{
					__result = typeof(PalmeraTree);
				}

				if (type_name == "RanchingRebalanced.Pacu.OutOfLiquidMonitor")
				{
					__result = typeof(RanchingRebalanced.Pacu.OutOfLiquidMonitor);
				}

				
			}
		}
	}
}