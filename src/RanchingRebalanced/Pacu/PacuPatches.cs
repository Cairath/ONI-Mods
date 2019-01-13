using System.Collections.Generic;
using Harmony;
using TUNING;
using UnityEngine;

namespace RanchingRebalanced.Pacu
{
	public class PacuPatches
	{
		[HarmonyPatch(typeof(BasePacuConfig), "CreatePrefab")]
		public static class BasePacuConfig_CreatePrefab_Patch
		{
			public static void Prefix()
			{
				PacuTuning.STANDARD_CALORIES_PER_CYCLE = 1000000f;
			}

			public static void Postfix(ref GameObject __result)
			{
				__result.AddOrGet<OutOfLiquidMonitor>();
			}
		}

		[HarmonyPatch(typeof(PacuConfig), "CreatePacu")]
		public static class PacuConfig_CreatePacu_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				var algaeKgPerDay = 50f;
				var kgPerDay = 150f;
				var calPerDay = PacuTuning.STANDARD_CALORIES_PER_CYCLE;

				var dietList = new List<Diet.Info>();
				DietUtils.AddToDiet(dietList, SimHashes.Algae.CreateTag(), SimHashes.ToxicSand.CreateTag(), calPerDay, algaeKgPerDay, 0.75f);
				DietUtils.AddToDiet(dietList, SimHashes.Phosphorite.CreateTag(), SimHashes.ToxicSand.CreateTag(), calPerDay, kgPerDay, 0.5f);
				DietUtils.AddToDiet(dietList, SimHashes.Fertilizer.CreateTag(), SimHashes.ToxicSand.CreateTag(), calPerDay, kgPerDay, 0.5f);

				__result = DietUtils.SetupDiet(__result, dietList, calPerDay / kgPerDay, 25f);
			}
		}

		[HarmonyPatch(typeof(PacuCleanerConfig), "CreatePacu")]
		public static class PacuCleanerConfig_CreatePacu_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				var algaeKgPerDay = 50f;
				var calPerDay = PacuTuning.STANDARD_CALORIES_PER_CYCLE;

				var dietList = new List<Diet.Info>();
				DietUtils.AddToDiet(dietList, SimHashes.Algae.CreateTag(), SimHashes.ToxicSand.CreateTag(), calPerDay, algaeKgPerDay, 0.75f);

				__result = DietUtils.SetupDiet(__result, dietList, calPerDay / algaeKgPerDay, 25f);
			}
		}

		[HarmonyPatch(typeof(PacuTropicalConfig))]
		[HarmonyPatch("CreatePacu")]
		public static class PacuTropicalConfig_CreatePacu_Patch
		{
			public static void Postfix(ref GameObject __result, bool is_baby)
			{
				var algaeKgPerDay = 50f;
				var kgPerDay = 150f;
				var calPerDay = PacuTuning.STANDARD_CALORIES_PER_CYCLE;

				var dietList = new List<Diet.Info>();
				DietUtils.AddToDiet(dietList, SimHashes.Algae.CreateTag(), SimHashes.ToxicSand.CreateTag(), calPerDay, algaeKgPerDay, 0.75f);
				DietUtils.AddToDiet(dietList, SimHashes.SlimeMold.CreateTag(), SimHashes.Algae.CreateTag(), calPerDay, kgPerDay, 0.33f);
				DietUtils.AddToDiet(dietList, FOOD.FOOD_TYPES.MEAT, SimHashes.ToxicSand.CreateTag(), calPerDay, kgPerDay);
				DietUtils.AddToDiet(dietList, FOOD.FOOD_TYPES.COOKEDMEAT, SimHashes.ToxicSand.CreateTag(), calPerDay, kgPerDay);

				__result = DietUtils.SetupDiet(__result, dietList, calPerDay / kgPerDay, 25f);

				if (is_baby) return;

				__result.AddComponent<Storage>().capacityKg = 10f;

				ElementConsumer elementConsumer = __result.AddOrGet<PassiveElementConsumer>();
				elementConsumer.elementToConsume = SimHashes.Water;
				elementConsumer.consumptionRate = 0.2f;
				elementConsumer.capacityKG = 10f;
				elementConsumer.consumptionRadius = 3;
				elementConsumer.showInStatusPanel = true;
				elementConsumer.sampleCellOffset = new Vector3(0.0f, 0.0f, 0.0f);
				elementConsumer.isRequired = false;
				elementConsumer.storeOnConsume = true;
				elementConsumer.showDescriptor = false;

				__result.AddOrGet<UpdateElementConsumerPosition>();

				var bubbleSpawner = __result.AddComponent<BubbleSpawner>();
				bubbleSpawner.element = SimHashes.DirtyWater;
				bubbleSpawner.emitMass = 2f;
				bubbleSpawner.emitVariance = 0.5f;
				bubbleSpawner.initialVelocity = new Vector2f(0, 1);

				var elementConverter = __result.AddOrGet<ElementConverter>();
				elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
				{
					new ElementConverter.ConsumedElement(SimHashes.Water.CreateTag(), 0.2f)
				};
				elementConverter.outputElements = new ElementConverter.OutputElement[1]
				{
					new ElementConverter.OutputElement(0.2f, SimHashes.DirtyWater, 0.0f, true, 0.0f, 0.5f, true)
				};
			}
		}

		[HarmonyPatch(typeof(PacuTropicalConfig))]
		[HarmonyPatch("OnSpawn")]
		public static class PacuTropicalConfig_OnSpawn_Patch
		{
			public static void Postfix(ref GameObject inst)
			{
				var component = inst.GetComponent<ElementConsumer>();
				if (component != null) component.EnableConsumption(true);
			}
		}
	}
}
