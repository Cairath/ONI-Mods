using System;
using Harmony;

namespace PalmeraTree
{
	public class PalmeraTreeMod
	{
		[HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
		public class PalmeraTreeEntityConfigManagerPatch
		{
			private static void Prefix()
			{
				TUNING.CROPS.CROP_TYPES.Add(new Crop.CropVal(PalmeraBerryConfig.ID, 800f, 1, true));
			}

			private static void Postfix()
			{
				object obj1 = Activator.CreateInstance(typeof(PalmeraBerryConfig));
				EntityConfigManager.Instance.RegisterEntity(obj1 as IEntityConfig);

				object obj = Activator.CreateInstance(typeof(PalmeraTreeConfig));
				EntityConfigManager.Instance.RegisterEntity(obj as IEntityConfig);

			
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
