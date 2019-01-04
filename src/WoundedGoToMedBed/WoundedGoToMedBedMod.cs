using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace WoundedGoToMedBed
{
	public class WoundedGoToMedBedMod
	{
		[HarmonyPatch(typeof(MedicalBedConfig), "DoPostConfigureComplete")]
		public class MedicalBedConfigDoPostConfigureComplete
		{
			private static void Postfix(ref GameObject go)
			{
				var clinic = go.GetComponent<Clinic>();
				clinic.healthEffect = "MedicalCot";
				clinic.doctoredHealthEffect = "MedicalCotDoctored";
			}
		}

		[HarmonyPatch(typeof(WoundMonitor), "InitializeStates")]
		public class WoundMonitorInitializeStates
		{
			private static void Postfix(ref WoundMonitor __instance)
			{
				__instance.wounded
					.Exit(UnassignClinic);

				__instance.wounded.light
					.ToggleUrge(Db.Get().Urges.Heal)
					.Update("AutoAssignClinic", ((smi, dt) => AutoAssignClinic(smi)), UpdateRate.SIM_4000ms);

				__instance.wounded.medium
					.ToggleUrge(Db.Get().Urges.Heal)
					.Update("AutoAssignClinic", ((smi, dt) => AutoAssignClinic(smi)), UpdateRate.SIM_4000ms);

				__instance.wounded.heavy
					.ToggleUrge(Db.Get().Urges.Heal)
					.Update("AutoAssignClinic", ((smi, dt) => AutoAssignClinic(smi)), UpdateRate.SIM_4000ms);
			}


			public static void AutoAssignClinic(WoundMonitor.Instance smi)
			{
				var soleOwner = smi.sm.masterTarget.Get(smi).GetComponent<MinionIdentity>().GetSoleOwner();
				var clinic = Db.Get().AssignableSlots.Clinic;
				var slot = soleOwner.GetSlot(clinic);
				if (slot == null || slot.assignable != null)
				{
					return;
				}
					
				soleOwner.AutoAssignSlot(clinic);
			}

			public static void UnassignClinic(WoundMonitor.Instance smi)
			{
				smi.sm.masterTarget.Get(smi).GetComponent<MinionIdentity>().GetSoleOwner().GetSlot(Db.Get().AssignableSlots.Clinic)?.Unassign();
			}
		}
	}
}
