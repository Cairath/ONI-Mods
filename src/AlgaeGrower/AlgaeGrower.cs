using UnityEngine;

namespace AlgaeGrower
{
	public class AlgaeGrower : StateMachineComponent<AlgaeGrower.SMInstance>
	{
		[SerializeField]
		public CellOffset pressureSampleOffset = CellOffset.none;

		[MyCmpGet]
		private Operational operational;

		protected override void OnPrefabInit()
		{
			GetComponent<KBatchedAnimController>().randomiseLoopedOffset = true;
			base.OnPrefabInit();
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();
			smi.StartSM();
		}

		public class SMInstance : GameStateMachine<States, SMInstance, AlgaeGrower, object>.GameInstance
		{
			private Operational operational;
			public ElementConverter converter;

			public SMInstance(AlgaeGrower master)
			  : base(master)
			{
				operational = master.GetComponent<Operational>();
				converter = master.GetComponent<ElementConverter>();
			}

			public bool HasEnoughMass(Tag tag)
			{
				return converter.HasEnoughMass(tag);
			}

			public bool IsOperational
			{
				get
				{
					if (operational.IsOperational)
						return true;
					return false;
				}
			}

			public bool HasLight()
			{
				int cell = Grid.PosToCell(smi.master.transform.GetPosition());
				return Grid.LightCount[cell] > 0;
			}
		}

		public class States : GameStateMachine<States, SMInstance, AlgaeGrower>
		{
			public State generatingOxygen;
			public State stoppedGeneratingOxygen;
			public State stoppedGeneratingOxygenTransition;
			public State noWater;
			public State noFert;
			public State gotFert;
			public State gotWater;
			public State lostFert;
			public State notoperational;
			public State noLight;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = notoperational;

				root
					.EventTransition(GameHashes.OperationalChanged, notoperational, smi => !smi.IsOperational);

				notoperational
					.QueueAnim("off")
					.EventTransition(GameHashes.OperationalChanged, noLight, smi => smi.IsOperational);

				noLight
					.QueueAnim("off")
					.Enter(smi => smi.master.operational.SetActive(false))
					.Update("NoLight", (smi, dt) => { if (smi.HasLight() && smi.HasEnoughMass(GameTags.Fertilizer)) smi.GoTo(gotFert); }, UpdateRate.SIM_1000ms);

				gotFert
					.PlayAnim("on_pre")
					.OnAnimQueueComplete(noWater);

				lostFert
					.PlayAnim("on_pst")
					.OnAnimQueueComplete(noFert);

				noFert
					.QueueAnim("off")
					.EventTransition(GameHashes.OnStorageChange, gotFert, smi => smi.HasEnoughMass(GameTags.Fertilizer))
					.Enter(smi => smi.master.operational.SetActive(false));

				noWater
					.QueueAnim("on")
					.Enter(smi => smi.master.GetComponent<PassiveElementConsumer>().EnableConsumption(true))
					.EventTransition(GameHashes.OnStorageChange, lostFert, smi => !smi.HasEnoughMass(GameTags.Fertilizer))
					.EventTransition(GameHashes.OnStorageChange, gotWater, smi =>
					{
						if (smi.HasEnoughMass(GameTags.Fertilizer))
							return smi.HasEnoughMass(GameTags.Water);
						return false;
					});

				gotWater
					.PlayAnim("working_pre")
					.OnAnimQueueComplete(generatingOxygen);

				generatingOxygen
					.Enter(smi => smi.master.operational.SetActive(true))
					.Exit(smi => smi.master.operational.SetActive(false))
					.QueueAnim("working_loop", true)
					.EventTransition(GameHashes.OnStorageChange, stoppedGeneratingOxygen,
						smi => !smi.HasEnoughMass(GameTags.Water) || !smi.HasEnoughMass(GameTags.Fertilizer))
					.Update("GeneratingOxygen", (smi, dt) => { if (!smi.HasLight()) smi.GoTo(stoppedGeneratingOxygen); }, UpdateRate.SIM_1000ms);

				stoppedGeneratingOxygen
					.PlayAnim("working_pst")
					.OnAnimQueueComplete(stoppedGeneratingOxygenTransition);

				stoppedGeneratingOxygenTransition
					.Update("StoppedGeneratingOxygenTransition", (smi, dt) => { if (!smi.HasLight()) smi.GoTo(noLight); }, UpdateRate.SIM_200ms)
					.EventTransition(GameHashes.OnStorageChange, noWater, smi => !smi.HasEnoughMass(GameTags.Water) && smi.HasLight())
					.EventTransition(GameHashes.OnStorageChange, lostFert, smi => !smi.HasEnoughMass(GameTags.Fertilizer) && smi.HasLight())
					.EventTransition(GameHashes.OnStorageChange, gotWater, smi =>
					{
						if (smi.HasEnoughMass(GameTags.Water) && smi.HasLight())
							return smi.HasEnoughMass(GameTags.Fertilizer);
						return false;
					});
			}
		}
	}
}