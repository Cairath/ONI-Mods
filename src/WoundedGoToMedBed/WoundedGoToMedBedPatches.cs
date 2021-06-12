using HarmonyLib;
using static CaiLib.Logger.Logger;

namespace WoundedGoToMedBed
{
	public class WoundedGoToMedBedPatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit();
			}
		}

		[HarmonyPatch(typeof(WoundMonitor))]
		[HarmonyPatch(nameof(WoundMonitor.InitializeStates))]
		public class WoundMonitorInitializeStates
		{
			public static void Postfix(ref WoundMonitor __instance)
			{
				__instance.wounded
					.Exit(UnassignClinic);

				__instance.wounded.light
					.ToggleUrge(Db.Get().Urges.Heal)
					.Update(nameof(AutoAssignClinic), ((smi, dt) => AutoAssignClinic(smi)), UpdateRate.SIM_1000ms);

				__instance.wounded.medium
					.ToggleUrge(Db.Get().Urges.Heal)
					.Update(nameof(AutoAssignClinic), ((smi, dt) => AutoAssignClinic(smi)), UpdateRate.SIM_1000ms);

				__instance.wounded.heavy
					.ToggleUrge(Db.Get().Urges.Heal)
					.Update(nameof(AutoAssignClinic), ((smi, dt) => AutoAssignClinic(smi)), UpdateRate.SIM_1000ms);
			}

			public static void AutoAssignClinic(WoundMonitor.Instance smi)
			{
				var soleOwner = smi.sm.masterTarget.Get(smi).GetComponent<MinionIdentity>().GetSoleOwner();

				var clinic = Db.Get().AssignableSlots.Clinic;
				var slot = soleOwner.GetSlot(clinic);

				if (slot == null || slot.assignable != null) return;

				soleOwner.AutoAssignSlot(clinic);
			}

			public static void UnassignClinic(WoundMonitor.Instance smi)
			{
				smi.sm.masterTarget.Get(smi).GetComponent<MinionIdentity>().GetSoleOwner().GetSlot(Db.Get().AssignableSlots.Clinic)?.Unassign();
			}
		}
	}
}
