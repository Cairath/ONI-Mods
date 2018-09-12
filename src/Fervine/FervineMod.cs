using System;
using Harmony;

namespace Fervine
{
	public class FervineMod
	{
		[HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
		public class PalmeraTreeEntityConfigManagerPatch
		{

			private static void Prefix()
			{
				Strings.Add("STRINGS.CREATURES.SPECIES.SEEDS.HEATBULB.NAME", FervineConfig.SeedName);
				Strings.Add("STRINGS.CREATURES.SPECIES.SEEDS.HEATBULB.DESC", FervineConfig.SeedDesc);

				var a = Assets.GetAnim("plantheatbulb_kanim");
				for (int i = 0; i < a.GetData().animCount; i++)
				{
					Debug.Log(a.GetData().GetAnim(i).name);
				}
			}

			private static void Postfix()
			{
				object heatbulb = Activator.CreateInstance(typeof(FervineConfig));
				EntityConfigManager.Instance.RegisterEntity(heatbulb as IEntityConfig);
			}
		}
	}
}