using System.Collections.Generic;
using Klei.AI;

namespace LightsOut
{
	public class Effects
	{
		public const string PitchBlackId = "PitchBlack";
		public const string DarkId = "Dark";

		//StressDelta
		//StaminaDelta
		//QualityOfLife -- new hope --morale?
		//BladderDelta
		//ImmuneLevelDelta
		//HitPointsDelta
		//BreathDelta

		//DiseaseCureSpeed
		//DoctoredLevel
		//"GermResistance"

		//Athletics
		//Decor
		//Learning
		//Machinery
		//Cooking
		//Construction
		//Strength
		//Caring
		//Art
		//Botanist
		//Ranching

		//MaturityDelta (plants)
		//"MachinerySpeed" (engies tune up)
		//WildnessDelta (ranching)
		//IncubationDelta
		//public Attribute Construction;
		//public Attribute Digging;
		//public Attribute Machinery;
		//public Attribute Athletics;
		//public Attribute Learning;
		//public Attribute Cooking;
		//public Attribute Caring;
		//public Attribute Strength;
		//public Attribute Art;
		//public Attribute Botanist;
		//public Attribute Ranching;

		private static List<string> AttributeIds = new List<string>
		{
			"Construction",
			"Digging",
			"Machinery",
			"Learning",
			"Cooking",
			"Strength",
			"Art",
			"Botanist",
			"Ranching"
		};

		private static class DebuffValues
		{
			public static class PitchBlack
			{
				public static Dictionary<DebuffTier, int> AthleticsDebuff = new Dictionary<DebuffTier, int>
				{
					{DebuffTier.None, 0},
					{DebuffTier.Light, -5},
					{DebuffTier.Harsh, -10}
				};

				public static Dictionary<DebuffTier, int> OtherStatsDebuff = new Dictionary<DebuffTier, int>
				{
					{DebuffTier.None, 0},
					{DebuffTier.Light, -3},
					{DebuffTier.Harsh, -5}
				};
			}

			public static class Dark
			{
				public static Dictionary<DebuffTier, int> AthleticsDebuff = new Dictionary<DebuffTier, int>
				{
					{DebuffTier.None, 0},
					{DebuffTier.Light, -3},
					{DebuffTier.Harsh, -5}
				};

				public static Dictionary<DebuffTier, int> OtherStatsDebuff = new Dictionary<DebuffTier, int>
				{
					{DebuffTier.None, 0},
					{DebuffTier.Light, -2},
					{DebuffTier.Harsh, -3}
				};
			}
		}

		public List<Effect> GenerateEffectsList()
		{
			return new List<Effect>
			{
				CreatePitchBlack(),
				CreateDark()
			};
		}

		private Effect CreatePitchBlack()
		{
			var config = LightsOutPatches.ConfigManager.Config;

			var pitchBlack = new Effect(PitchBlackId, "Pitch Black", "This Duplicant can't see anything!", 0f, true, true, true)
			{
				SelfModifiers = new List<AttributeModifier>()
			};

			pitchBlack.SelfModifiers.Add(new AttributeModifier("Athletics", DebuffValues.PitchBlack.AthleticsDebuff[config.DebuffTier]));

			foreach (var attributeId in AttributeIds)
			{
				pitchBlack.SelfModifiers.Add(new AttributeModifier(attributeId, DebuffValues.PitchBlack.OtherStatsDebuff[config.DebuffTier]));
			}

			return pitchBlack;
		}

		private Effect CreateDark()
		{
			var config = LightsOutPatches.ConfigManager.Config;

			var dark = new Effect(DarkId, "Dark", "This Duplicant is in a hard place and can't see well!", 0f, true, true, true)
			{
				SelfModifiers = new List<AttributeModifier>()
			};

			dark.SelfModifiers.Add(new AttributeModifier("Athletics", DebuffValues.Dark.AthleticsDebuff[config.DebuffTier]));

			foreach (var attributeId in AttributeIds)
			{
				dark.SelfModifiers.Add(new AttributeModifier(attributeId, DebuffValues.Dark.OtherStatsDebuff[config.DebuffTier]));
			}

			return dark;
		}
	}
}