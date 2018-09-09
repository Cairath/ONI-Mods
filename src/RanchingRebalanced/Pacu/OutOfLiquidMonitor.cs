using System;
using Harmony;
using KSerialization;
using UnityEngine;
using STRINGS;

namespace RanchingRebalanced.Pacu
{
	public class OutOfLiquidMonitor : KMonoBehaviour, ISim1000ms
	{
		[Serialize]
		[SerializeField]
		private float timeToSuffocate;

		[Serialize]
		private bool suffocated;
		private bool suffocating;

		protected const float MaxSuffocateTime = 50f;
		protected const float RegenRate = 5f;
		protected const float CellLiquidThreshold = 0.35f;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.timeToSuffocate = 15f;
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.CheckDryingOut();
		}

		private void CheckDryingOut()
		{
			if (this.suffocated || this.GetComponent<KPrefabID>().HasTag(GameTags.Trapped))
				return;

			if (!IsInWater(Grid.PosToCell(this.gameObject.transform.GetPosition())))
			{
				if (!this.suffocating)
				{
					this.suffocating = true;
					this.Trigger((int) GameHashes.DryingOut);
				}

				if ((double) this.timeToSuffocate > 0.0)
					return;

				DeathMonitor.Instance smi = this.GetSMI<DeathMonitor.Instance>();
				if (smi != null)
					smi.Kill(Db.Get().Deaths.Suffocation);

				this.Trigger((int) GameHashes.DriedOut);
				this.suffocated = true;
			}
			else
			{
				if (!this.suffocating)
					return;

				this.suffocating = false;
				this.Trigger((int) GameHashes.EnteredWetArea, (object) null);
			}
		}

		public bool Suffocating => this.suffocating;

		private static bool IsInWater(int cell)
		{
			return Grid.IsSubstantialLiquid(cell, CellLiquidThreshold) || Grid.IsSubstantialLiquid(Grid.CellBelow(cell), 0.5f);
		}

		public void Sim1000ms(float dt)
		{
			this.CheckDryingOut();

			if (this.suffocating)
			{
				if (this.suffocated)
					return;

				this.timeToSuffocate -= dt;

				if ((double) this.timeToSuffocate > 0.0)
					return;

				this.CheckDryingOut();
			}
			else
			{
				this.timeToSuffocate += dt * RegenRate;
				this.timeToSuffocate = Mathf.Clamp(this.timeToSuffocate, 0.0f, MaxSuffocateTime);
			}
		}
	}

	[HarmonyPatch(typeof(Manager), "GetType", new[] { typeof(string) })]
	public static class OutOfLiquidMonitorSerializationPatch
	{
		public static void Postfix(string type_name, ref Type __result)
		{
			if (type_name == "RanchingRebalanced.Pacu.OutOfLiquidMonitor")
			{
				__result = typeof(OutOfLiquidMonitor);
			}
		}
	}
}