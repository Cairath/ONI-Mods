using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace WoundedGoToMedBed
{
	public class WoundedGoToMedBedMod
	{
		[HarmonyPatch(typeof(WoundMonitor), "InitializeStates")]
		public class Patch1
		{
			private static void Postfix(ref WoundMonitor __instance)
			{
				__instance.wounded.light
					.ToggleUrge(Db.Get().Urges.RestDueToDisease)
					.Update("AutoAssignClinic", ((smi, dt) => AutoAssignClinic(smi)), UpdateRate.SIM_4000ms)
					.Exit(smi => UnassignClinic(smi));

				__instance.wounded.medium.ToggleAnims("anim_loco_wounded_kanim", 1f);
				__instance.wounded.heavy.ToggleAnims("anim_loco_wounded_kanim", 3f);
			}


			public static void AutoAssignClinic(WoundMonitor.Instance smi)
			{
				Ownables component = smi.sm.masterTarget.Get(smi).GetComponent<Ownables>();
				AssignableSlot clinic = Db.Get().AssignableSlots.Clinic;
				AssignableSlotInstance slot = component.GetSlot(clinic);
				if (slot == null || (Object)slot.assignable != (Object)null)
					return;
				component.AutoAssignSlot(clinic);
			}

			public static void UnassignClinic(WoundMonitor.Instance smi)
			{
				AssignableSlotInstance slot = smi.sm.masterTarget.Get(smi).GetComponent<Ownables>().GetSlot(Db.Get().AssignableSlots.Clinic);
				if (slot == null)
					return;
				slot.Unassign(true);
			}
		}
	}
}
