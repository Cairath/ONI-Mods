using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using Klei.AI;
using TUNING;
using UnityEngine;

namespace RanchingRebalanced.Pacu
{
	public class Pacus
	{
		[HarmonyPatch(typeof(BasePacuConfig), "CreatePrefab")]
		public class BasePacuConfigCreatePrefab
		{
			private static void Prefix()
			{
				PacuTuning.STANDARD_CALORIES_PER_CYCLE = 1000000f;
			}

			private static void Postfix(ref GameObject __result)
			{
				__result.AddOrGet<OutOfLiquidMonitor>();
			}
		}

		[HarmonyPatch(typeof(PacuConfig), "CreatePacu")]
		public class PacuConfigCreatePacu
		{
			private static void Postfix(ref GameObject __result)
			{
				float algaeKgPerDay = 50f;
				float kgPerDay = 150f;
				float calPerDay = PacuTuning.STANDARD_CALORIES_PER_CYCLE;

				var dietList = new List<Diet.Info>();
				DietUtils.AddToDiet(dietList, SimHashes.Algae.CreateTag(), SimHashes.ToxicSand.CreateTag(), calPerDay, algaeKgPerDay, 0.75f);
				DietUtils.AddToDiet(dietList, SimHashes.Phosphorite.CreateTag(), SimHashes.ToxicSand.CreateTag(), calPerDay, kgPerDay, 0.5f);
				DietUtils.AddToDiet(dietList, SimHashes.Fertilizer.CreateTag(), SimHashes.ToxicSand.CreateTag(), calPerDay, kgPerDay, 0.5f);

				__result = DietUtils.SetupDiet(__result, dietList, calPerDay / kgPerDay, 25f);
			}
		}

		[HarmonyPatch(typeof(PacuCleanerConfig), "CreatePacu")]
		public class PacuCleanerConfigCreatePacu
		{
			private static void Postfix(ref GameObject __result)
			{
				float algaeKgPerDay = 50f;
				float calPerDay = PacuTuning.STANDARD_CALORIES_PER_CYCLE;

				var dietList = new List<Diet.Info>();
				DietUtils.AddToDiet(dietList, SimHashes.Algae.CreateTag(), SimHashes.ToxicSand.CreateTag(), calPerDay, algaeKgPerDay, 0.75f);

				__result = DietUtils.SetupDiet(__result, dietList, calPerDay / algaeKgPerDay, 25f);
			}
		}

		[HarmonyPatch(typeof(PacuTropicalConfig), "CreatePacu")]
		public class PacuTropicalConfigCreatePacu
		{
			private static void Postfix(ref GameObject __result, bool is_baby)
			{
				float algaeKgPerDay = 50f;
				float kgPerDay = 150f;
				float calPerDay = PacuTuning.STANDARD_CALORIES_PER_CYCLE;

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
				BubbleSpawner bubbleSpawner = __result.AddComponent<BubbleSpawner>();
				bubbleSpawner.element = SimHashes.DirtyWater;
				bubbleSpawner.emitMass = 2f;
				bubbleSpawner.emitVariance = 0.5f;
				bubbleSpawner.initialVelocity = new Vector2f(0, 1);
				ElementConverter elementConverter = __result.AddOrGet<ElementConverter>();
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

		[HarmonyPatch(typeof(PacuTropicalConfig), "OnSpawn")]
		public class PacuTropicalConfigOnSpawn
		{
			private static void Postfix(ref GameObject inst)
			{
				ElementConsumer component = inst.GetComponent<ElementConsumer>();
				if (component != null) component.EnableConsumption(true);
			}
		}
	}
}