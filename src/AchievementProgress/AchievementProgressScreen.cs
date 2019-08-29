using System;
using System.Collections.Generic;
using System.Text;
using TUNING;

namespace AchievementProgress
{
	public class AchievementProgressScreen
	{
		private enum Achievement
		{
			Dress8Dupes,
			Build4Parks,
			Tubular,
			DiscoverMap,
			VentOxygen,
			HatchPoop,
			SustainablePower,
			Carnivore,
			TuneUp,
			Locavore
		}

		private static Dictionary<Achievement, string> Achievements = new Dictionary<Achievement, string>
		{
			{ Achievement.Dress8Dupes, "And Nowhere to Go"},
			{ Achievement.Build4Parks, "Some Reservations"},
			{ Achievement.Tubular, "Totally Tubular"},
			{ Achievement.DiscoverMap, "Pulling Back The Veil"},
			{ Achievement.VentOxygen, "Oxygen Not Occluded"},
			{ Achievement.HatchPoop, "Down the Hatch"},
			{ Achievement.SustainablePower, "Super Sustainable"},
			{ Achievement.Carnivore, "Carnivore"},
			{ Achievement.TuneUp, "Finely Tuned Machine"},
			{ Achievement.Locavore, "Locavore"}
		};

		public static void CreateScreen(PauseScreen pauseScreen)
		{
			var builder = new StringBuilder();

			var eligible = IsEligible(out var reason);
			var eligibleString = eligible ? "<color=#00ff00>eligible</color>" : $"<color=#ff0000>not eligible</color> ({reason}).";
			builder.AppendFormat("Achievement eligibility: {0}\n\n", eligibleString);

			foreach (var key in Achievements.Keys)
			{
				builder.AppendFormat("<b>{0}</b>: {1}\n", Achievements[key], GetAchievementProgress(key));
			}

			((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(
					ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject,
					pauseScreen.transform.parent.gameObject))
				.PopupConfirmDialog(builder.ToString(),
					() => { pauseScreen.gameObject.SetActive(true); },
					null,
					confirm_text: "CLOSE",
					title_text: "Achievement Progress");
		}

		private static string GetAchievementProgress(Achievement achievement)
		{
			switch (achievement)
			{
				case Achievement.Dress8Dupes:
					return CheckDress8Dupes();
				case Achievement.Build4Parks:
					return CheckBuild4NatureReserves();
				case Achievement.Tubular:
					return CheckTubular();
				case Achievement.DiscoverMap:
					return CheckReveal();
				case Achievement.VentOxygen:
					return CheckOxygenVent();
				case Achievement.HatchPoop:
					return CheckHatchPoop();
				case Achievement.SustainablePower:
					return CheckSustainablePower();
				case Achievement.Carnivore:
					return CheckCarnivore();
				case Achievement.TuneUp:
					return CheckTuneUp();
				case Achievement.Locavore:
					return CheckLocavore();
				default:
					throw new ArgumentOutOfRangeException(nameof(achievement), achievement, null);
			}
		}

		private static bool IsEligible(out string failReason)
		{
			failReason = string.Empty;

			if (DebugHandler.InstantBuildMode)
			{
				failReason += "Instant Build Mode is enabled";
				return false;
			}

			if (SaveGame.Instance.sandboxEnabled)
			{
				failReason += "sandbox is enabled";
				return false;
			}

			if (Game.Instance.debugWasUsed)
			{
				failReason += "debug was used";
				return false;
			}

			return true;
		}

		private static string FormatProgress(float current, float goal, string unit)
		{
			var percentage = Math.Min(100, current / goal * 100);
			var progress = $"{current:N0} {unit} / {goal:N0} {unit} ({percentage:0.00}%)";

			if (percentage == 100)
			{
				progress = $"<color=#00ff00>{progress}</color>";
			}

			return progress;
		}

		private static string CheckDress8Dupes()
		{
			var num = 0;
			var goal = 8;

			foreach (var minionIdentity in Components.MinionIdentities.Items)
			{
				var equipment = minionIdentity.GetEquipment();
				if (equipment != null && equipment.IsSlotOccupied(Db.Get().AssignableSlots.Outfit))
					++num;
			}

			return FormatProgress(num, goal, string.Empty);
		}

		private static string CheckBuild4NatureReserves()
		{
			var num = 0;
			var goal = 4;

			foreach (var room in Game.Instance.roomProber.rooms)
			{
				if (room.roomType == Db.Get().RoomTypes.NatureReserve)
					++num;
			}

			return FormatProgress(num, goal, string.Empty);
		}

		private static string CheckTubular()
		{
			var num = 0;
			var goal = 10000;

			foreach (var component1 in Components.MinionIdentities.Items)
			{
				var component2 = component1.GetComponent<Navigator>();
				if ((component2 != null && component2.distanceTravelledByNavType.ContainsKey(NavType.Tube)))
					num += component2.distanceTravelledByNavType[NavType.Tube];
			}

			return FormatProgress(num, goal, "m");
		}

		private static string CheckHatchPoop()
		{
			float num = 0;
			var goal = 10000;

			var poopTag = "HatchMetal";

			if (Game.Instance.savedInfo.creaturePoopAmount.ContainsKey(poopTag))
				num = Game.Instance.savedInfo.creaturePoopAmount[poopTag];

			return FormatProgress(num, goal, string.Empty);
		}

		private static string CheckSustainablePower()
		{
			var num = 0.0f;
			var goal = 240000f;

			var disallowedBuildings = new List<Tag> {
				 "MethaneGenerator",
				 "PetroleumGenerator",
				 "WoodGasGenerator",
				 "Generator"
			};

			var failed = false;

			foreach (var disallowedBuilding in disallowedBuildings)
			{
				if (Game.Instance.savedInfo.powerCreatedbyGeneratorType.ContainsKey(disallowedBuilding))
					failed = true;
			}

			if (!failed)
			{
				foreach (var keyValuePair in Game.Instance.savedInfo.powerCreatedbyGeneratorType)
				{
					if (!disallowedBuildings.Contains(keyValuePair.Key))
						num += keyValuePair.Value;
				}
			}

			var kJ = num / 1000;

			return !failed
				? FormatProgress(kJ, goal, "kJ")
				: "<color=#ff0000>failed</color>";
		}

		private static string CheckReveal()
		{
			var num = 0.0f;
			var goal = 0.8f;

			for (var index = 0; index < Grid.Visible.Length; ++index)
			{
				if (Grid.Visible[index] > 0)
					++num;
			}

			var explored = num / (double)Grid.Visible.Length;
			return $"{(explored * 100):0.00}% / {goal * 100}%";
		}

		private static string CheckTuneUp()
		{
			float num = 0;
			var goal = 100;

			var entry = ReportManager.Instance.TodaysReport.GetEntry(ReportManager.ReportType.ChoreStatus);
			for (var index = 0; index < entry.contextEntries.Count; ++index)
			{
				var contextEntry = entry.contextEntries[index];
				if (contextEntry.context == Db.Get().ChoreTypes.PowerTinker.Name)
					num += contextEntry.Negative;
			}

			for (var index1 = 0; index1 < ReportManager.Instance.reports.Count; ++index1)
			{
				for (var index2 = 0; index2 < ReportManager.Instance.reports[index1].GetEntry(ReportManager.ReportType.ChoreStatus).contextEntries.Count; ++index2)
				{
					var contextEntry = ReportManager.Instance.reports[index1].GetEntry(ReportManager.ReportType.ChoreStatus).contextEntries[index2];
					if (contextEntry.context == Db.Get().ChoreTypes.PowerTinker.Name)
						num += contextEntry.Negative;
				}
			}

			return FormatProgress(Math.Abs(num), goal, string.Empty);
		}

		private static string CheckOxygenVent()
		{
			float num = 0;
			var goal = 1000;

			foreach (var network in Conduit.GetNetworkManager(ConduitType.Gas).GetNetworks())
			{
				if (network is FlowUtilityNetwork flowUtilityNetwork)
				{
					foreach (var sink in flowUtilityNetwork.sinks)
					{
						var component = sink.GameObject.GetComponent<Vent>();
						if (component != null)
							num += component.GetVentedMass(SimHashes.Oxygen);
					}
				}
			}

			return FormatProgress(num, goal, "kg");
		}

		private static string CheckCarnivore()
		{
			var goal = 400000;
			var foods = new List<string>
			{
				FOOD.FOOD_TYPES.MEAT.Id,
				FOOD.FOOD_TYPES.FISH_MEAT.Id,
				FOOD.FOOD_TYPES.COOKED_MEAT.Id,
				FOOD.FOOD_TYPES.COOKED_FISH.Id,
				FOOD.FOOD_TYPES.SURF_AND_TURF.Id,
				FOOD.FOOD_TYPES.BURGER.Id
			};

			var cal = RationTracker.Get().GetCaloiresConsumedByFood(foods);
			var kcal = cal / 1000f;

			var before100 = GameClock.Instance.GetCycle() + 1 <= 100;

			return before100 ? FormatProgress(kcal, goal, "kcal")
				: "<color=#ff0000>failed</color>";
		}

		private static string CheckLocavore()
		{
			var goal = 400000;

			var cal = RationTracker.Get().GetCaloriesConsumed();
			var kcal = cal / 1000f;

			var hasPlantables = false;

			foreach (var plantablePlot in Components.PlantablePlots.Items)
			{
				if (plantablePlot.Occupant != null)
				{
					foreach (var depositObjectTag in plantablePlot.possibleDepositObjectTags)
					{
						if (depositObjectTag != GameTags.DecorSeed)
							hasPlantables = true;
						break;
					}
				}
			}

			return !hasPlantables ? FormatProgress(kcal, goal, "kcal")
				: "<color=#ff0000>failed</color>";
		}
	}
}