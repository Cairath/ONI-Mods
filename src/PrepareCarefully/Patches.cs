using System;
using System.Collections.Generic;
using Harmony;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

namespace PrepareCarefully
{
    public static class Patches
    {
	    [HarmonyPatch(typeof(SplashMessageScreen))]
	    [HarmonyPatch("OnPrefabInit")]
	    public static class SplashMessageScreen_OnPrefabInit_Patch
	    {
		    public static void Postfix()
		    {
			  //  CaiLib.ModCounter.ModCounter.Hit(ModInfo.Name, ModInfo.Version);
			 //   CaiLib.Logger.LogInit(ModInfo.Name, ModInfo.Version);
		    }
	    }
	   
		 [HarmonyPatch(typeof(MinionStartingStats), new Type[] { typeof(bool) })]
		 [HarmonyPatch(MethodType.Constructor)]
		public static class PersonalityGen
		{
			public static void Postfix(ref MinionStartingStats __instance)
			{
				__instance.personality = Generate();
				__instance.voiceIdx = UnityEngine.Random.Range(0, 4);
				__instance.Name = __instance.personality.Name;
				__instance.NameStringKey = __instance.personality.nameStringKey;
				__instance.GenderStringKey = __instance.personality.genderStringKey;
				__instance.Traits.Add(Db.Get().traits.Get(MinionConfig.MINION_BASE_TRAIT_ID));

				

				//Traverse.Create(__instance).Method("GenerateAptitudes").GetValue();
				//__instance.GenerateAptitudes();
				GenerateAptitudes(__instance);
				GenerateTraits(__instance);
				GenerateAttributes(__instance);
				//__instance.GenerateAttributes(__instance.GenerateTraits(is_starter_minion, disabled_chore_groups), disabled_chore_groups);
				KCompBuilder.BodyData bodyData = MinionStartingStats.CreateBodyData(__instance.personality);
				foreach (AccessorySlot resource in Db.Get().AccessorySlots.resources)
				{
					if (resource.accessories.Count != 0)
					{
						Accessory accessory = (Accessory)null;
						if (resource == Db.Get().AccessorySlots.HeadShape)
						{
							accessory = resource.Lookup(bodyData.headShape);
							if (accessory == null)
								__instance.personality.headShape = 0;
						}
						else if (resource == Db.Get().AccessorySlots.Mouth)
						{
							accessory = resource.Lookup(bodyData.mouth);
							if (accessory == null)
								__instance.personality.mouth = 0;
						}
						else if (resource == Db.Get().AccessorySlots.Eyes)
						{
							accessory = resource.Lookup(bodyData.eyes);
							if (accessory == null)
								__instance.personality.eyes = 0;
						}
						else if (resource == Db.Get().AccessorySlots.Hair)
						{
							accessory = resource.Lookup(bodyData.hair);
							if (accessory == null)
								__instance.personality.hair = 0;
						}
						else if (resource != Db.Get().AccessorySlots.HatHair)
						{
							if (resource == Db.Get().AccessorySlots.Body)
							{
								accessory = resource.Lookup(bodyData.body);
								if (accessory == null)
									__instance.personality.body = 0;
							}
							else if (resource == Db.Get().AccessorySlots.Arm)
								accessory = resource.Lookup(bodyData.arms);
						}
						if (accessory == null)
							accessory = resource.accessories[0];
						__instance.accessories.Add(accessory);
					}
				}
			}

			private static void GenerateAptitudes(MinionStartingStats instance)
			{
				instance.roleAptitudes.Clear();

				//var l = Game.Instance.roleManager.RolesConfigs;
				//foreach (var role in l)
				//{
				//	Debug.Log($"id: {role.id}  name: {role.name} propername: {role.GetProperName()} group: {role.roleGroup.ToString()}");
				//}

					instance.roleAptitudes.Add("Farming", 1f);
					instance.roleAptitudes.Add("MedicalAid", 1f);
			}

			private static void GenerateTraits(MinionStartingStats instance)
			{
				instance.stressTrait = Db.Get().traits.Get(instance.personality.stresstrait);
				instance.congenitaltrait = null;

				instance.Traits.Clear();

				DUPLICANTSTATS.TraitVal traitVal = DUPLICANTSTATS.GOODTRAITS.Find(x => x.id == "StrongArm");
				Trait trait2 = Db.Get().traits.TryGet(traitVal.id);

				if (trait2 == null) Debug.LogWarning("Trying to add nonexistent trait: " + traitVal.id);

				instance.Traits.Add(trait2);
				instance.Traits.Add(Db.Get().traits.Get(MinionConfig.MINION_BASE_TRAIT_ID));
			}

			private static void GenerateAttributes(MinionStartingStats instance)
			{
				instance.StartingLevels["Strength"] = 0;
				instance.StartingLevels["Caring"] = 0;
				instance.StartingLevels["Construction"] = 0;
				instance.StartingLevels["Digging"] = 0;
				instance.StartingLevels["Machinery"] = 0;
				instance.StartingLevels["Learning"] = 0;
				instance.StartingLevels["Cooking"] = 0;
				instance.StartingLevels["Botanist"] = 0;
				instance.StartingLevels["Art"] = 0;
				instance.StartingLevels["Ranching"] = 0;
				instance.StartingLevels["Athletics"] = 0;
			}

			private static Personality Generate()
		    {
				//stress traits:
				//Aggressive
				//StressVomiter
				//UglyCrier
				//BingeEater
				
				//CongenitalTrait: always None

				//personality types:
				//Doofy
				//Cool
				//Grumpy
				//Sweet
			    var p = new Personality("Ari", "Test Meep", "Female", "Doofy", "Aggressive", "None", 1, 1, 1, 1, 2, 1, "Some description" );
			    return p;
		    }
	    }

		//[HarmonyPatch(typeof(CharacterContainer), "Initialize")]
	 //   public class MinionStartingStatsOnSpawn
	 //   {
		//    public static void Postfix()
		//    {
		//	    Debug.Log("CharacterContainer koop: ");
		//    }
	 //   }

	 //   [HarmonyPatch(typeof(CharacterContainer), "GenerateCharacter")]
	 //   public class MinionStartingStatsOnSpawnGenerateCharacter
	 //   {
		//    public static void Prefix()
		//    {
		//	    Debug.Log("generate: ");
		//    }
	 //   }

	 //   //   [HarmonyPatch(typeof(CharacterSelectionController), "InitializeContainers")]
	 //   [HarmonyPatch(typeof(MinionSelectScreen), "OnPrefabInit")]
	 //   public class GeneShuffler
	 //   {
		//    private static void Prefix(MinionSelectScreen __instance)
		//    {
		//	    var availableCharCount = Traverse.Create(__instance).Field("availableCharCount").SetValue(7);
		//	    var selectableCharCount = Traverse.Create(__instance).Field("selectableCharCount").SetValue(7);

		//	    Debug.Log("availableCharCount: " + availableCharCount);
		//	    Debug.Log("selectableCharCount: " + selectableCharCount);

		//    }

		//    private static void Postfix(MinionSelectScreen __instance)
		//    {
		//	    var availableCharCount = Traverse.Create(__instance).Field("availableCharCount").GetValue<int>();
		//	    var selectableCharCount = Traverse.Create(__instance).Field("selectableCharCount").GetValue<int>();

		//	    Debug.Log("availableCharCount: " + availableCharCount);
		//	    Debug.Log("selectableCharCount: " + selectableCharCount);
		//    }
	 //   }

	    [HarmonyPatch(typeof(CharacterContainer), "IsCharacterRedundant")]
	    public class CharacterContainerIsCharacterRedundant
	    {
		    private static bool Prefix(ref bool __result)
		    {
			    __result = false;

			    return false;
		    }
	    }
    }
}
