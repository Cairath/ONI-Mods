using Harmony;
using Steamworks;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace RanchingRebalanced.Hatches
{
	public class Hatches
	{
		[HarmonyPatch(typeof(HatchMetalConfig), "CreateHatch")]
		public class HatchMetalConfigCreateHatch
		{
			private static void Postfix(ref GameObject __result)
			{
				float kgPerDay = 100f;
				float calPerDay = HatchTuning.STANDARD_CALORIES_PER_CYCLE;

				var dietList = new List<Diet.Info>();
				dietList.AddRange(BaseHatchConfig.MetalDiet(GameTags.Metal, calPerDay / kgPerDay, 0.75f, (string)null, 0.0f));
				DietUtils.AddToDiet(dietList, SimHashes.Carbon.CreateTag(), SimHashes.Diamond.CreateTag(), calPerDay, kgPerDay, 0.1f);

				__result = DietUtils.SetupDiet(__result, dietList, calPerDay / kgPerDay, 10f);
			}
		}

		[HarmonyPatch(typeof(HatchVeggieConfig), "CreateHatch")]
		public class HatchVeggieConfigCreateHatch
		{
			private static void Postfix(ref GameObject __result)
			{
				float kgPerDay = 140f;
				float calPerDay = HatchTuning.STANDARD_CALORIES_PER_CYCLE;

				var dietList = new List<Diet.Info>();
				dietList.AddRange(BaseHatchConfig.VeggieDiet(SimHashes.Lime.CreateTag(), calPerDay / kgPerDay, 1f, (string) null, 0.0f));
				dietList.AddRange(DietUtils.CreateFoodDiet(SimHashes.Lime.CreateTag(), calPerDay, kgPerDay));

				__result = DietUtils.SetupDiet(__result, dietList, calPerDay/kgPerDay, 25f);
			}
		}

		[HarmonyPatch(typeof(HatchHardConfig), "CreateHatch")]
		public class HatchHardConfigCreateHatch
		{
			private static void Postfix(ref GameObject __result)
			{
				float kgPerDay = 140f;
				float calPerDay = HatchTuning.STANDARD_CALORIES_PER_CYCLE;

				var dietList = new List<Diet.Info>();
				dietList.AddRange(BaseHatchConfig.HardRockDiet(SimHashes.Carbon.CreateTag(), calPerDay / kgPerDay, 0.75f, (string)null, 0.0f));

				DietUtils.AddToDiet(dietList, SimHashes.Copper.CreateTag(), SimHashes.Cuprite.CreateTag(), calPerDay, kgPerDay, 0.75f);
				DietUtils.AddToDiet(dietList, SimHashes.Gold.CreateTag(), SimHashes.GoldAmalgam.CreateTag(), calPerDay, kgPerDay, 0.75f);
				DietUtils.AddToDiet(dietList, SimHashes.Iron.CreateTag(), SimHashes.IronOre.CreateTag(), calPerDay, kgPerDay, 0.75f);
				DietUtils.AddToDiet(dietList, SimHashes.Tungsten.CreateTag(), SimHashes.Wolframite.CreateTag(), calPerDay, kgPerDay, 0.75f);

				__result = DietUtils.SetupDiet(__result, dietList, calPerDay / kgPerDay, 25f);
			}
		}

		[HarmonyPatch(typeof(HatchConfig), "CreateHatch")]
		public class HatchConfigCreateHatch
		{
			private static void Postfix(ref GameObject __result)
			{
				float kgPerDay = 140f;
				float regolithKgPerDay = 280f;
				float calPerDay = HatchTuning.STANDARD_CALORIES_PER_CYCLE;

				var dietList = new List<Diet.Info>();
				dietList.AddRange(BaseHatchConfig.BasicRockDiet(SimHashes.Carbon.CreateTag(), calPerDay / kgPerDay, 0.5f, (string)null, 0.0f));
				dietList.AddRange(DietUtils.CreateFoodDiet(SimHashes.Carbon.CreateTag(), calPerDay, kgPerDay * 0.75f));
				DietUtils.AddToDiet(dietList, SimHashes.Regolith.CreateTag(), SimHashes.Carbon.CreateTag(), calPerDay, regolithKgPerDay);

				__result = DietUtils.SetupDiet(__result, dietList, calPerDay / kgPerDay, 25f);
			}
		}
	}
}