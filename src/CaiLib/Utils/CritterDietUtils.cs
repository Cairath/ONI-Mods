using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace CaiLib.Utils
{
	public class CritterDietUtils
	{
		public static void AddToDiet(List<Diet.Info> dietInfos, HashSet<Tag> consumedTags, Tag poopTag, float dailyCalories,
			float dailyKilograms, float conversionRate = 1.0f, string diseaseId = "", float diseasePerKg = 0.0f)
		{
			dietInfos.Add(string.IsNullOrEmpty(diseaseId)
				? new Diet.Info(consumedTags, poopTag, dailyCalories / dailyKilograms, conversionRate)
				: new Diet.Info(consumedTags, poopTag, dailyCalories / dailyKilograms, conversionRate, diseaseId, diseasePerKg));
		}

		public static void AddToDiet(List<Diet.Info> dietInfos, Tag consumedTag, Tag poopTag, float dailyCalories,
			float dailyKilograms, float conversionRate = 1.0f, string diseaseId = "", float diseasePerKg = 0.0f)
		{
			AddToDiet(dietInfos, new HashSet<Tag>(new[] { consumedTag }), poopTag, dailyCalories, dailyKilograms, conversionRate, diseaseId, diseasePerKg);
		}

		public static void AddToDiet(List<Diet.Info> dietInfos, EdiblesManager.FoodInfo foodInfo, Tag poopTag, float dailyCalories,
			float howManyKgOfPoopForDailyCalories = 0f, string diseaseId = "", float diseasePerKg = 0.0f)
		{
			var caloriesInKgOfFood = foodInfo.CaloriesPerUnit;
			var kgOfFoodToSatisfyCalories = dailyCalories / caloriesInKgOfFood;

			var conversionRatio = 1f / (kgOfFoodToSatisfyCalories / howManyKgOfPoopForDailyCalories);

			dietInfos.Add(string.IsNullOrEmpty(diseaseId)
				? new Diet.Info(new HashSet<Tag>(new[] { new Tag(foodInfo.Id) }), poopTag, caloriesInKgOfFood,
					conversionRatio)
				: new Diet.Info(new HashSet<Tag>(new[] { new Tag(foodInfo.Id) }), poopTag, caloriesInKgOfFood,
					conversionRatio, diseaseId, diseasePerKg));
		}

		public static GameObject SetupPooplessDiet(GameObject prefab, List<Diet.Info> dietInfos)
		{
			var diet = new Diet(dietInfos.ToArray());
			prefab.AddOrGetDef<CreatureCalorieMonitor.Def>().diet = diet;
			prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;

			return prefab;
		}

		public static GameObject SetupDiet(GameObject prefab, List<Diet.Info> dietInfos, float referenceCaloriesPerKg, float minPoopSizeInKg)
		{
			var diet = new Diet(dietInfos.ToArray());

			var def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
			def.diet = diet;
			def.minPoopSizeInCalories = referenceCaloriesPerKg * minPoopSizeInKg;

			prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;

			return prefab;
		}

		public static List<Diet.Info> CreateFoodDiet(Tag poopTag, float calPerDay, float poopKgPerDay)
		{
			var dietList = new List<Diet.Info>();
			foreach (var foodType in EdiblesManager.GetAllFoodTypes())
			{
				if (foodType.CaloriesPerUnit > 0.0)
					AddToDiet(dietList, foodType, poopTag, calPerDay, poopKgPerDay);
			}

			return dietList;
		}
	}
}