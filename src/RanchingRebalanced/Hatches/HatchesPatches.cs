using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace RanchingRebalanced.Hatches
{
	public class HatchesPatches
	{
		[HarmonyPatch(typeof(HatchMetalConfig))]
		[HarmonyPatch("CreateHatch")]
		public static class HatchMetalConfig_CreateHatch_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				var kgPerDay = 100f;
				var calPerDay = HatchTuning.STANDARD_CALORIES_PER_CYCLE;

				var dietList = new List<Diet.Info>();
				dietList.AddRange(BaseHatchConfig.MetalDiet(GameTags.Metal, calPerDay / kgPerDay, 0.75f, null, 0.0f));
				DietUtils.AddToDiet(dietList, SimHashes.Carbon.CreateTag(), SimHashes.Diamond.CreateTag(), calPerDay, kgPerDay, 0.1f);

				__result = DietUtils.SetupDiet(__result, dietList, calPerDay / kgPerDay, 10f);
			}
		}

		[HarmonyPatch(typeof(HatchVeggieConfig))]
		[HarmonyPatch("CreateHatch")]
		public static class HatchVeggieConfig_CreateHatch_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				var kgPerDay = 140f;
				var calPerDay = HatchTuning.STANDARD_CALORIES_PER_CYCLE;

				var dietList = new List<Diet.Info>();
				dietList.AddRange(BaseHatchConfig.VeggieDiet(SimHashes.Lime.CreateTag(), calPerDay / kgPerDay, 1f, null, 0.0f));
				dietList.AddRange(DietUtils.CreateFoodDiet(SimHashes.Lime.CreateTag(), calPerDay, kgPerDay));

				__result = DietUtils.SetupDiet(__result, dietList, calPerDay / kgPerDay, 25f);
			}
		}

		[HarmonyPatch(typeof(HatchHardConfig))]
		[HarmonyPatch("CreateHatch")]
		public static class HatchHardConfig_CreateHatch_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				var kgPerDay = 140f;
				var calPerDay = HatchTuning.STANDARD_CALORIES_PER_CYCLE;

				var dietList = new List<Diet.Info>();
				dietList.AddRange(BaseHatchConfig.HardRockDiet(SimHashes.Carbon.CreateTag(), calPerDay / kgPerDay, 0.75f, null, 0.0f));

				DietUtils.AddToDiet(dietList, SimHashes.Copper.CreateTag(), SimHashes.Cuprite.CreateTag(), calPerDay, kgPerDay, 0.75f);
				DietUtils.AddToDiet(dietList, SimHashes.Gold.CreateTag(), SimHashes.GoldAmalgam.CreateTag(), calPerDay, kgPerDay, 0.75f);
				DietUtils.AddToDiet(dietList, SimHashes.Iron.CreateTag(), SimHashes.IronOre.CreateTag(), calPerDay, kgPerDay, 0.75f);
				DietUtils.AddToDiet(dietList, SimHashes.Tungsten.CreateTag(), SimHashes.Wolframite.CreateTag(), calPerDay, kgPerDay, 0.75f);

				__result = DietUtils.SetupDiet(__result, dietList, calPerDay / kgPerDay, 25f);
			}
		}

		[HarmonyPatch(typeof(HatchConfig))]
		[HarmonyPatch("CreateHatch")]
		public static class HatchConfig_CreateHatch_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				var kgPerDay = 140f;
				var regolithKgPerDay = 280f;
				var calPerDay = HatchTuning.STANDARD_CALORIES_PER_CYCLE;

				var dietList = new List<Diet.Info>();
				dietList.AddRange(BaseHatchConfig.BasicRockDiet(SimHashes.Carbon.CreateTag(), calPerDay / kgPerDay, 0.5f, null, 0.0f));
                dietList.AddRange(BaseHatchConfig.MetalDiet(SimHashes.Carbon.CreateTag(), calPerDay / kgPerDay, 0.5f, null, 0.0f));
                dietList.AddRange(DietUtils.CreateFoodDiet(SimHashes.Carbon.CreateTag(), calPerDay, kgPerDay * 0.75f));
				DietUtils.AddToDiet(dietList, SimHashes.Regolith.CreateTag(), SimHashes.Carbon.CreateTag(), calPerDay, regolithKgPerDay);

				__result = DietUtils.SetupDiet(__result, dietList, calPerDay / kgPerDay, 25f);
			}
		}
	}
}