using System;
using Harmony;
using KSerialization;
using UnityEngine;

namespace RanchingRebalanced.Pacu
{
	public class OutOfLiquidMonitor : KMonoBehaviour, ISim1000ms
	{
		[Serialize]
		[SerializeField]
		private float _timeToSuffocate;

		[Serialize]
		private bool _suffocated;
		private bool _suffocating;

		protected const float MaxSuffocateTime = 50f;
		protected const float RegenRate = 5f;
		protected const float CellLiquidThreshold = 0.35f;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			_timeToSuffocate = 15f;
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();
			CheckDryingOut();
		}

		private void CheckDryingOut()
		{
			if (_suffocated || GetComponent<KPrefabID>().HasTag(GameTags.Trapped))
				return;

			if (!IsInWater(Grid.PosToCell(gameObject.transform.GetPosition())))
			{
				if (!_suffocating)
				{
					_suffocating = true;
					Trigger((int)GameHashes.DryingOut);
				}

				if ((double)_timeToSuffocate > 0.0)
					return;

				DeathMonitor.Instance smi = this.GetSMI<DeathMonitor.Instance>();
				smi?.Kill(Db.Get().Deaths.Suffocation);

				Trigger((int)GameHashes.DriedOut);
				_suffocated = true;
			}
			else
			{
				if (!_suffocating)
					return;

				_suffocating = false;
				Trigger((int)GameHashes.EnteredWetArea, (object)null);
			}
		}

		public bool Suffocating => _suffocating;

		private static bool IsInWater(int cell)
		{
			return Grid.IsSubstantialLiquid(cell, CellLiquidThreshold) || Grid.IsSubstantialLiquid(Grid.CellBelow(cell), 0.5f);
		}

		public void Sim1000ms(float dt)
		{
			CheckDryingOut();

			if (_suffocating)
			{
				if (_suffocated)
					return;

				_timeToSuffocate -= dt;

				if (_timeToSuffocate > 0.0)
					return;

				CheckDryingOut();
			}
			else
			{
				_timeToSuffocate += dt * RegenRate;
				_timeToSuffocate = Mathf.Clamp(_timeToSuffocate, 0.0f, MaxSuffocateTime);
			}
		}
	}

	[HarmonyPatch(typeof(KSerialization.Manager))]
	[HarmonyPatch("GetType")]
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