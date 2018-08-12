﻿using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace RanchingRebalanced.Pacu
{
	public class TropicalPacuMods
	{
		/***
		 *  Adds Water->PWater conversion
		 */
		[HarmonyPatch(typeof(PacuTropicalConfig), "CreatePacu")]
		public class CreatePacu
		{
			private static void Postfix(ref GameObject __result, bool is_baby)
			{
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
		public class OnSpawn
		{
			private static void Postfix(ref GameObject inst)
			{
				ElementConsumer component = inst.GetComponent<ElementConsumer>();
				if (component != null) component.EnableConsumption(true);
			}
		}

		[HarmonyPatch(typeof(BaseHatchConfig), "VeggieDiet")]
		public class PoopPatch
		{
			private static void Postfix(ref List<Diet.Info> __result)
			{
				var dietInfo = __result[0];
				TagBits consumed_tag_bits = new TagBits();
				consumed_tag_bits.SetTag(TagManager.Create("ColdBreatherSeed",
					(string) STRINGS.CREATURES.SPECIES.SEEDS.COLDBREATHER.NAME));
				var newDiet = new Diet.Info(consumed_tag_bits, SimHashes.Lime.CreateTag(), dietInfo.caloriesPerKg, dietInfo.producedConversionRate);
				__result.Clear();
				__result.Add(newDiet);
			}
		}
	}
}