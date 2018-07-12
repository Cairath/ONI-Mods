using System;
using UnityEngine;

namespace PipedAlgaeTerrarium
{
	public class PipedAlgaeTerrarium : StateMachineComponent<PipedAlgaeTerrarium.SMInstance>
	{
		[SerializeField]
		public float lightBonusMultiplier = 1.1f;
		public CellOffset pressureSampleOffset = CellOffset.none;

		protected override void OnPrefabInit()
		{
			this.GetComponent<KBatchedAnimController>().randomiseLoopedOffset = true;
			base.OnPrefabInit();
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.smi.StartSM();
		}

		public class SMInstance : GameStateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.GameInstance
		{
			private Operational operational;
			public ElementConverter converter;
			private ConduitDispenser dispenser;

			public SMInstance(PipedAlgaeTerrarium master)
			  : base(master)
			{
				this.operational = master.GetComponent<Operational>();
				this.converter = master.GetComponent<ElementConverter>();
				this.dispenser = master.GetComponent<ConduitDispenser>();
			}

			public bool HasEnoughMass(Tag tag)
			{
				return this.converter.HasEnoughMass(tag);
			}

			public bool IsOperational
			{
				get
				{
					if (this.operational.IsOperational && this.dispenser.IsConnected)
						return true;
					return false;
				}
			}
		}

		public class States : GameStateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium>
		{
			public GameStateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.State generatingOxygen;
			public GameStateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.State stoppedGeneratingOxygen;
			public GameStateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.State stoppedGeneratingOxygenTransition;
			public GameStateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.State noWater;
			public GameStateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.State noAlgae;
			public GameStateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.State gotAlgae;
			public GameStateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.State gotWater;
			public GameStateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.State lostAlgae;
			public GameStateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.State notoperational;

			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = (StateMachine.BaseState)this.notoperational;

				this.root
					.EventTransition(GameHashes.OperationalChanged, this.notoperational, (StateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.Transition.ConditionCallback)
						(smi => !smi.IsOperational));

				this.notoperational
					.QueueAnim("off", false, (Func<PipedAlgaeTerrarium.SMInstance, string>)null)
					.EventTransition(GameHashes.OperationalChanged, this.noAlgae, (StateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.Transition.ConditionCallback)
					(smi => smi.IsOperational));

				this.gotAlgae.PlayAnim("on_pre").OnAnimQueueComplete(this.noWater);
				this.lostAlgae.PlayAnim("on_pst").OnAnimQueueComplete(this.noAlgae);

				this.noAlgae
					.QueueAnim("off", false, (Func<PipedAlgaeTerrarium.SMInstance, string>)null)
					.EventTransition(GameHashes.OnStorageChange, this.gotAlgae, (StateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.Transition.ConditionCallback)
						(smi => smi.HasEnoughMass(GameTags.Algae)))
					.Enter((StateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.State.Callback)
						(smi => smi.master.operational.SetActive(false, false)));


				this.noWater
					.QueueAnim("on", false, (Func<PipedAlgaeTerrarium.SMInstance, string>)null)
					.Enter((StateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.State.Callback)
						(smi => smi.master.GetComponent<PassiveElementConsumer>().EnableConsumption(true)))
					.EventTransition(GameHashes.OnStorageChange, this.lostAlgae, (StateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.Transition.ConditionCallback)
						(smi => !smi.HasEnoughMass(GameTags.Algae)))
					.EventTransition(GameHashes.OnStorageChange, this.gotWater, (StateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.Transition.ConditionCallback)
						(smi =>
						{
							if (smi.HasEnoughMass(GameTags.Algae))
								return smi.HasEnoughMass(GameTags.Water);
							return false;
						}));

				this.gotWater.PlayAnim("working_pre").OnAnimQueueComplete(this.generatingOxygen);

				this.generatingOxygen
					.Enter((StateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.State.Callback)
						(smi => smi.master.operational.SetActive(true, false)))
					.Exit((StateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.State.Callback)
						(smi => smi.master.operational.SetActive(false, false)))
					.Update("GeneratingOxygen", (System.Action<PipedAlgaeTerrarium.SMInstance, float>)
						((smi, dt) =>
						{
							int cell = Grid.PosToCell(smi.master.transform.GetPosition());
							smi.converter.OutputMultiplier = Grid.LightCount[cell] <= 0 ? 1f : smi.master.lightBonusMultiplier;
						}), UpdateRate.SIM_200ms, false).QueueAnim("working_loop", true, (Func<PipedAlgaeTerrarium.SMInstance, string>)null)
					.EventTransition(GameHashes.OnStorageChange, this.stoppedGeneratingOxygen, (StateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.Transition.ConditionCallback)
						(smi => !smi.HasEnoughMass(GameTags.Water) || !smi.HasEnoughMass(GameTags.Algae)));

				this.stoppedGeneratingOxygen
					.PlayAnim("working_pst")
					.OnAnimQueueComplete(this.stoppedGeneratingOxygenTransition);

				this.stoppedGeneratingOxygenTransition
					.EventTransition(GameHashes.OnStorageChange, this.noWater, (StateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.Transition.ConditionCallback)
						(smi => !smi.HasEnoughMass(GameTags.Water)))
					.EventTransition(GameHashes.OnStorageChange, this.lostAlgae, (StateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.Transition.ConditionCallback)
						(smi => !smi.HasEnoughMass(GameTags.Algae)))
					.EventTransition(GameHashes.OnStorageChange, this.gotWater, (StateMachine<PipedAlgaeTerrarium.States, PipedAlgaeTerrarium.SMInstance, PipedAlgaeTerrarium, object>.Transition.ConditionCallback)
						(smi =>
						{
							if (smi.HasEnoughMass(GameTags.Water))
								return smi.HasEnoughMass(GameTags.Algae);
							return false;
						}));
			}
		}
	}
}