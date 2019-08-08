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
				var completed = GetAchievementProgress(key, out var failed, out var progress);

				builder.AppendFormat("<b>{0}</b>: {1}\n", Achievements[key], progress);
			}

			((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(
					ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject,
					pauseScreen.transform.parent.gameObject))
				.PopupConfirmDialog(builder.ToString(),
					() => { pauseScreen.gameObject.SetActive(true); },
					() => { pauseScreen.gameObject.SetActive(true); },
					confirm_text: "Ok",
					title_text: "Achievement Progress",
					cancel_text: "Not Ok");
		}

		private static bool GetAchievementProgress(Achievement achievement, out bool failed, out string progress)
		{
			switch (achievement)
			{
				case Achievement.Dress8Dupes:
					return CheckDress8Dupes(out failed, out progress);
				case Achievement.Build4Parks:
					return CheckBuild4NatureReserves(out failed, out progress);
				case Achievement.Tubular:
					return CheckTubular(out failed, out progress);
				case Achievement.DiscoverMap:
					return CheckReveal(out failed, out progress);
				case Achievement.VentOxygen:
					return CheckOxygenVent(out failed, out progress);
				case Achievement.HatchPoop:
					return CheckHatchPoop(out failed, out progress);
				case Achievement.SustainablePower:
					return CheckSustainablePower(out failed, out progress);
				case Achievement.Carnivore:
					return CheckCarnivore(out failed, out progress);
				case Achievement.TuneUp:
					return CheckTuneUp(out failed, out progress);
				case Achievement.Locavore:
					return CheckLocavore(out failed, out progress);
				default:
					throw new ArgumentOutOfRangeException(nameof(achievement), achievement, null);
			}
		}

		private static bool IsEligible(out string failReason)
		{
			failReason = string.Empty;
			var eligible = true;

			if (DebugHandler.InstantBuildMode)
			{
				eligible = false;
				failReason += "Instant Build Mode is enabled";
				return false;
			}

			if (SaveGame.Instance.sandboxEnabled)
			{
				eligible = false;
				failReason += "sandbox is enabled";
				return false;
			}

			if (Game.Instance.debugWasUsed)
			{
				eligible = false;
				failReason += "debug was used";
				return false;
			}

			return true;
		}

		private static bool CheckDress8Dupes(out bool failed, out string progress)
		{
			var num = 0;
			var goal = 8;

			foreach (var minionIdentity in Components.MinionIdentities.Items)
			{
				var equipment = minionIdentity.GetEquipment();
				if (equipment != null && equipment.IsSlotOccupied(Db.Get().AssignableSlots.Outfit))
					++num;
			}

			failed = false;
			progress = $"{num} / {goal} ({(num / (float)goal * 100):N0}%)";

			return num >= 8;
		}

		private static bool CheckBuild4NatureReserves(out bool failed, out string progress)
		{
			var num = 0;
			var goal = 4;

			foreach (var room in Game.Instance.roomProber.rooms)
			{
				if (room.roomType == Db.Get().RoomTypes.NatureReserve)
					++num;
			}

			failed = false;
			progress = $"{num} / {goal} ({(num / (float)goal * 100):N0}%)";

			return num >= 4;
		}

		private static bool CheckTubular(out bool failed, out string progress)
		{
			var num = 0;
			var goal = 10000;

			foreach (var component1 in Components.MinionIdentities.Items)
			{
				var component2 = component1.GetComponent<Navigator>();
				if ((component2 != null && component2.distanceTravelledByNavType.ContainsKey(NavType.Tube)))
					num += component2.distanceTravelledByNavType[NavType.Tube];
			}

			failed = false;
			progress = $"{num} m / {goal} m ({(num / (float)goal * 100):N0}%)";
			return num >= goal;
		}

		private static bool CheckHatchPoop(out bool failed, out string progress)
		{
			float num = 0;
			var goal = 10000;

			var poopTag = "HatchMetal";

			if (Game.Instance.savedInfo.creaturePoopAmount.ContainsKey(poopTag))
				num = Game.Instance.savedInfo.creaturePoopAmount[poopTag];

			failed = false;
			progress = $"{num} kg / {goal} kg ({(num / (float)goal * 100):N0}%)";
			return num >= goal;
		}

		private static bool CheckSustainablePower(out bool failed, out string progress)
		{
			var num = AchievementProgressPatches.SustainableEnergyCurrent;
			var goal = 240000f;

			var entry = ReportManager.Instance.TodaysReport.GetEntry(ReportManager.ReportType.EnergyCreated);

			failed = (Math.Abs(entry.Net) > 0.1f && Math.Abs(num) < 1f) || AchievementProgressPatches.SustainableEnergyFailed || AchievementProgressPatches.SustainableEnergyUsedDisallowedBuilding;

			progress = !failed
				? $"{num / 1000f} kJ / {goal} kJ ({(num / goal * 100):N0}%)"
				: "<color=#ff0000>failed</color>";

			return !failed && num / 1000f >= goal;
		}

		private static bool CheckReveal(out bool failed, out string progress)
		{
			var num = 0.0f;
			var goal = 0.8f;

			for (var index = 0; index < Grid.Visible.Length; ++index)
			{
				if (Grid.Visible[index] > 0)
					++num;
			}

			failed = false;
			var explored = num / (double) Grid.Visible.Length;
			progress = $"{(explored * 100):0.00}% / {goal * 100}%";

			return explored > goal;
		}

		private static bool CheckTuneUp(out bool failed, out string progress)
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

			failed = false;
			progress = $"{num} / {goal} ({(num / (float)goal * 100):N0}%)";

			return num >= goal;
		}

		private static bool CheckOxygenVent(out bool failed, out string progress)
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

			failed = false;
			progress = $"{num:0.00} kg / {goal} kg ({(num / (float)goal * 100):N0}%)";
			return num >= goal;
		}

		private static bool CheckCarnivore(out bool failed, out string progress)
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

			failed = false;

			progress = before100 ? $"{kcal} kcal / {goal} kcal ({(kcal / (float)goal * 100):N0}%)"
				: $"<color=#ff0000>can no longer be achieved - past cycle 100 </color> - {kcal} kcal / {goal} kcal ({(kcal / (float)goal * 100):N0}%)";
			return kcal >= goal;
		}

		private static bool CheckLocavore(out bool failed, out string progress)
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

			failed = hasPlantables;

			progress = $"{kcal} kcal / {goal} kcal ({(kcal / (float)goal * 100):N0}%)";
			return kcal >= goal;
		}
	}
}