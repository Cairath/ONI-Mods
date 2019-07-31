using UnityEngine;

namespace PipedAlgaeTerrarium
{
	public class PipedAlgaeTerrarium : StateMachineComponent<PipedAlgaeTerrarium.SMInstance>
	{
		[SerializeField]
		public float LightBonusMultiplier = 1.1f;
		public CellOffset PressureSampleOffset = CellOffset.none;

		[MyCmpGet]
#pragma warning disable 649
		private Operational operational;
#pragma warning restore 649

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

		public class SMInstance : GameStateMachine<States, SMInstance, PipedAlgaeTerrarium, object>.GameInstance
		{
			public readonly ElementConverter Converter;

			private readonly Operational _operational;
			private readonly ConduitDispenser _dispenser;

			public SMInstance(PipedAlgaeTerrarium master) : base(master)
			{
				Converter = master.GetComponent<ElementConverter>();
				_operational = master.GetComponent<Operational>();
				_dispenser = master.GetComponent<ConduitDispenser>();
			}

			public bool HasEnoughMass(Tag tag) => Converter.HasEnoughMass(tag);

			public bool IsOperational => _operational.IsOperational && _dispenser.IsConnected;
		}

		public class States : GameStateMachine<States, SMInstance, PipedAlgaeTerrarium>
		{
			public State GeneratingOxygen;
			public State StoppedGeneratingOxygen;
			public State StoppedGeneratingOxygenTransition;
			public State NoWater;
			public State NoAlgae;
			public State GotAlgae;
			public State GotWater;
			public State LostAlgae;
			public State NotOperational;

			public override void InitializeStates(out BaseState defaultState)
			{
				defaultState = NotOperational;

				root
					.EventTransition(GameHashes.OperationalChanged, NotOperational, smi => !smi.IsOperational);

				NotOperational
					.QueueAnim("off")
					.EventTransition(GameHashes.OperationalChanged, NoAlgae, smi => smi.IsOperational);

				GotAlgae
					.PlayAnim("on_pre").OnAnimQueueComplete(NoWater);

				LostAlgae
					.PlayAnim("on_pst").OnAnimQueueComplete(NoAlgae);

				NoAlgae
					.QueueAnim("off")
					.EventTransition(GameHashes.OnStorageChange, GotAlgae, smi => smi.HasEnoughMass(GameTags.Algae))
					.Enter(smi => smi.master.operational.SetActive(false));

				NoWater
					.QueueAnim("on")
					.Enter(smi => smi.master.GetComponent<PassiveElementConsumer>().EnableConsumption(true))
					.EventTransition(GameHashes.OnStorageChange, LostAlgae, smi => !smi.HasEnoughMass(GameTags.Algae))
					.EventTransition(GameHashes.OnStorageChange, GotWater, smi => smi.HasEnoughMass(GameTags.Algae) && smi.HasEnoughMass(GameTags.Water));

				GotWater
					.PlayAnim("working_pre").OnAnimQueueComplete(GeneratingOxygen);

				GeneratingOxygen
					.Enter(smi => smi.master.operational.SetActive(true))
					.Exit(smi => smi.master.operational.SetActive(false))
					.Update("GeneratingOxygen", (smi, dt) =>
					{
						var cell = Grid.PosToCell(smi.master.transform.GetPosition());
						smi.Converter.OutputMultiplier = Grid.LightCount[cell] <= 0 ? 1f : smi.master.LightBonusMultiplier;
					})
					.QueueAnim("working_loop", true)
					.EventTransition(GameHashes.OnStorageChange, StoppedGeneratingOxygen, smi => !smi.HasEnoughMass(GameTags.Water) || !smi.HasEnoughMass(GameTags.Algae));

				StoppedGeneratingOxygen
					.PlayAnim("working_pst")
					.OnAnimQueueComplete(StoppedGeneratingOxygenTransition);

				StoppedGeneratingOxygenTransition
					.EventTransition(GameHashes.OnStorageChange, NoWater, smi => !smi.HasEnoughMass(GameTags.Water))
					.EventTransition(GameHashes.OnStorageChange, LostAlgae, smi => !smi.HasEnoughMass(GameTags.Algae))
					.EventTransition(GameHashes.OnStorageChange, GotWater, smi => smi.HasEnoughMass(GameTags.Water) && smi.HasEnoughMass(GameTags.Algae));
			}
		}
	}
}
