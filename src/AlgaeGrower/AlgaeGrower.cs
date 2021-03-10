using UnityEngine;

namespace AlgaeGrower
{
	public class AlgaeGrower : StateMachineComponent<AlgaeGrower.SMInstance>
	{
		[SerializeField]
		public CellOffset PressureSampleOffset = CellOffset.none;

		[MyCmpGet]
#pragma warning disable 649
		private readonly Operational operational;
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

		public class SMInstance : GameStateMachine<States, SMInstance, AlgaeGrower, object>.GameInstance
		{
			private readonly Operational _operational;
			private readonly ElementConverter _converter;

			public SMInstance(AlgaeGrower master) : base(master)
			{
				_operational = master.GetComponent<Operational>();
				_converter = master.GetComponent<ElementConverter>();
			}

			public bool HasEnoughMass(Tag tag) => _converter.HasEnoughMass(tag);

			public bool IsOperational => _operational.IsOperational;

			public bool HasLight()
			{
				var cell = Grid.PosToCell(smi.master.transform.GetPosition());
				return Grid.LightCount[cell] > 0;
			}

			public void convertCo2InStorage(){
				Storage storage = smi.master.GetComponent<Storage>();
				float carbonMass = storage.GetMassAvailable(GameTags.CarbonDioxide);
				storage.ConsumeIgnoringDisease(GameTags.CarbonDioxide, carbonMass);
				storage.AddGasChunk(SimHashes.Oxygen, carbonMass, 303.15f, byte.MaxValue, 0, true);
				storage.Drop(GameTags.Oxygen);
			}
		}

		public class States : GameStateMachine<States, SMInstance, AlgaeGrower>
		{
			public State GeneratingOxygen;
			public State StoppedGeneratingOxygen;
			public State StoppedGeneratingOxygenTransition;
			public State NoWater;
			public State NoFert;
			public State GotFert;
			public State GotWater;
			public State LostFert;
			public State NotOperational;
			public State NoLight;

			public override void InitializeStates(out BaseState defaultState)
			{
				defaultState = NotOperational;

				root
					.EventTransition(GameHashes.OperationalChanged, NotOperational, smi => !smi.IsOperational);

				NotOperational
					.QueueAnim("off")
					.EventTransition(GameHashes.OperationalChanged, NoLight, smi => smi.IsOperational);

				NoLight
					.QueueAnim("off")
					.Enter(smi => smi.master.operational.SetActive(false))
					.Update("NoLight", (smi, dt) => { if (smi.HasLight() && smi.HasEnoughMass(GameTags.Agriculture)) smi.GoTo(GotFert); }, UpdateRate.SIM_1000ms);

				GotFert
					.PlayAnim("on_pre")
					.OnAnimQueueComplete(NoWater);

				LostFert
					.PlayAnim("on_pst")
					.OnAnimQueueComplete(NoFert);

				NoFert
					.QueueAnim("off")
					.EventTransition(GameHashes.OnStorageChange, GotFert, smi => smi.HasEnoughMass(GameTags.Agriculture))
					.Enter(smi => smi.master.operational.SetActive(false));

				NoWater
					.QueueAnim("on")
					.Enter(smi => smi.master.GetComponent<PassiveElementConsumer>().EnableConsumption(true))
					.EventTransition(GameHashes.OnStorageChange, LostFert, smi => !smi.HasEnoughMass(GameTags.Agriculture))
					.EventTransition(GameHashes.OnStorageChange, GotWater, smi => smi.HasEnoughMass(GameTags.Agriculture) && smi.HasEnoughMass(GameTags.Water));

				GotWater
					.PlayAnim("working_pre")
					.OnAnimQueueComplete(GeneratingOxygen);

				GeneratingOxygen
					.Enter(smi => smi.master.operational.SetActive(true))
					.Exit(smi => smi.master.operational.SetActive(false))
					.QueueAnim("working_loop", true)
					.EventTransition(GameHashes.OnStorageChange, StoppedGeneratingOxygen,
						smi => !smi.HasEnoughMass(GameTags.Water) || !smi.HasEnoughMass(GameTags.Agriculture))
					.Update("GeneratingOxygen", (smi, dt) => {
						if (!smi.HasLight()){
							smi.GoTo(StoppedGeneratingOxygen);
                        }
						else{
							smi.convertCo2InStorage();
						}
					}, UpdateRate.SIM_1000ms);

				StoppedGeneratingOxygen
					.PlayAnim("working_pst")
					.OnAnimQueueComplete(StoppedGeneratingOxygenTransition);

				StoppedGeneratingOxygenTransition
					.Update("StoppedGeneratingOxygenTransition", (smi, dt) => { if (!smi.HasLight()) smi.GoTo(NoLight); }, UpdateRate.SIM_200ms)
					.EventTransition(GameHashes.OnStorageChange, NoWater, smi => !smi.HasEnoughMass(GameTags.Water) && smi.HasLight())
					.EventTransition(GameHashes.OnStorageChange, LostFert, smi => !smi.HasEnoughMass(GameTags.Agriculture) && smi.HasLight())
					.EventTransition(GameHashes.OnStorageChange, GotWater, smi => smi.HasEnoughMass(GameTags.Water) && smi.HasLight() &&
					                                                              smi.HasEnoughMass(GameTags.Agriculture));
			}
		}
	}
}
