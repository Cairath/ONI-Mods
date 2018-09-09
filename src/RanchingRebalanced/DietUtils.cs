using System;
using System.Collections.Generic;
using UnityEngine;

namespace RanchingRebalanced
{
	public class DietUtils
	{
		public static void AddToDiet(List<Diet.Info> dietInfos, HashSet<Tag> consumedTags, Tag poopTag, float dailyCalories,
			float dailyKilograms, float conversionRate = 1.0f, string diseaseId = "", float diseasePerKg = 0.0f)
		{
			dietInfos.Add(String.IsNullOrEmpty(diseaseId)
				? new Diet.Info(consumedTags, poopTag, dailyCalories / dailyKilograms, conversionRate)
				: new Diet.Info(consumedTags, poopTag, dailyCalories / dailyKilograms, conversionRate, diseaseId, diseasePerKg));
		}

		public static void AddToDiet(List<Diet.Info> dietInfos, Tag consumedTag, Tag poopTag, float dailyCalories,
			float dailyKilograms, float conversionRate = 1.0f, string diseaseId = "", float diseasePerKg = 0.0f)
		{
			AddToDiet(dietInfos, new HashSet<Tag>((IEnumerable<Tag>)new Tag[] { consumedTag }), poopTag, dailyCalories, dailyKilograms, conversionRate, diseaseId, diseasePerKg);
		}

		public static void AddToDiet(List<Diet.Info> dietInfos, EdiblesManager.FoodInfo foodInfo, Tag poopTag, float dailyCalories,
			float howManyKgOfPoopForDailyCalories = 0f, string diseaseId = "", float diseasePerKg = 0.0f)
		{
			var caloriesInKgOfFood = foodInfo.CaloriesPerUnit;
			var kgOfFoodToSatisfyCalories = dailyCalories / caloriesInKgOfFood;

			var conversionRatio = 1f / (kgOfFoodToSatisfyCalories / howManyKgOfPoopForDailyCalories);

			if (String.IsNullOrEmpty(diseaseId))
			{
				dietInfos.Add(new Diet.Info(new HashSet<Tag>((IEnumerable<Tag>)new Tag[] { new Tag(foodInfo.Id) }), poopTag, caloriesInKgOfFood, conversionRatio));
			}
			else
			{
				dietInfos.Add(new Diet.Info(new HashSet<Tag>((IEnumerable<Tag>)new Tag[] { new Tag(foodInfo.Id) }), poopTag, caloriesInKgOfFood, conversionRatio, diseaseId, diseasePerKg));
			}
		}

		public static GameObject SetupPooplessDiet(GameObject prefab, List<Diet.Info> diet_infos)
		{
			Diet diet = new Diet(diet_infos.ToArray());
			prefab.AddOrGetDef<CreatureCalorieMonitor.Def>().diet = diet;
			prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
			return prefab;
		}

		public static GameObject SetupDiet(GameObject prefab, List<Diet.Info> diet_infos, float referenceCaloriesPerKg, float minPoopSizeInKg)
		{
			Diet diet = new Diet(diet_infos.ToArray());
			CreatureCalorieMonitor.Def def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
			def.diet = diet;
			def.minPoopSizeInCalories = referenceCaloriesPerKg * minPoopSizeInKg;
			prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
			return prefab;
		}

		public static List<Diet.Info> CreateFoodDiet(Tag poopTag, float calPerDay, float poopKgPerDay)
		{
			List<Diet.Info> dietList = new List<Diet.Info>();
			foreach (EdiblesManager.FoodInfo foodType in TUNING.FOOD.FOOD_TYPES_LIST)
			{
				if (foodType.CaloriesPerUnit > 0.0)
					DietUtils.AddToDiet(dietList, foodType, poopTag, calPerDay, poopKgPerDay);
			}

			return dietList;
		}
	}
}