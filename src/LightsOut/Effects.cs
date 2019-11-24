using System.Collections.Generic;
using Klei.AI;

namespace LightsOut
{
	public class Effects
	{
		public const string PitchBlackId = "PitchBlack";
		public const string DarkId = "Dark";

		#region attributes
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

		//MaturityDelta (plants)
		//"MachinerySpeed" (engies tune up)
		//WildnessDelta (ranching)
		//IncubationDelta

		//Decor

		//Athletics
		//Learning
		//Machinery
		//Cooking
		//Construction
		//Strength
		//Caring
		//Art
		//Botanist
		//Ranching
		//Digging
		#endregion

		private static readonly List<string> AttributeIds = new List<string>
		{
			"Construction",
			"Digging",
			"Machinery",
			"Learning",
			"Cooking",
			"Strength",
			"Art",
			"Botanist",
			"Ranching",
			"Caring"
		};

		private static class DebuffValues
		{
			public static class PitchBlack
			{
				public static readonly Dictionary<DebuffTier, int> AthleticsDebuff = new Dictionary<DebuffTier, int>
				{
					{DebuffTier.None, 0},
					{DebuffTier.Light, -5},
					{DebuffTier.Harsh, -10}
				};

				public static readonly Dictionary<DebuffTier, int> OtherStatsDebuff = new Dictionary<DebuffTier, int>
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

		public static List<Effect> GenerateEffectsList(DebuffTier tier)
		{	
			return new List<Effect>
			{
				CreatePitchBlack(tier),
				CreateDark(tier)
			};
		}

		private static Effect CreatePitchBlack(DebuffTier tier)
		{
			var pitchBlack = new Effect(PitchBlackId, "Pitch Black", "This Duplicant can't see anything!", 0f, true, false, true)
			{
				SelfModifiers = new List<AttributeModifier>()
			};

			pitchBlack.SelfModifiers.Add(new AttributeModifier("Athletics", DebuffValues.PitchBlack.AthleticsDebuff[tier]));

			foreach (var attributeId in AttributeIds)
			{
				pitchBlack.SelfModifiers.Add(new AttributeModifier(attributeId, DebuffValues.PitchBlack.OtherStatsDebuff[tier]));
			}

			return pitchBlack;
		}

		private static Effect CreateDark(DebuffTier tier)
		{
			var dark = new Effect(DarkId, "Dark", "This Duplicant is in a dark place and can't see well!", 0f, true, false, true)
			{
				SelfModifiers = new List<AttributeModifier>()
			};

			dark.SelfModifiers.Add(new AttributeModifier("Athletics", DebuffValues.Dark.AthleticsDebuff[tier]));

			foreach (var attributeId in AttributeIds)
			{
				dark.SelfModifiers.Add(new AttributeModifier(attributeId, DebuffValues.Dark.OtherStatsDebuff[tier]));
			}

			return dark;
		}
	}
}