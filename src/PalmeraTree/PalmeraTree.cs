using STRINGS;

namespace PalmeraTree
{
	public class PalmeraTree : StateMachineComponent<PalmeraTree.StatesInstance>
	{
		[MyCmpReq]
		private Crop _crop;

		[MyCmpReq]
		private WiltCondition _wiltCondition;

		[MyCmpReq]
		private Growing _growing;

		[MyCmpReq]
		private Harvestable _harvestable;

		[MyCmpReq]
		private ElementEmitter _elementEmitter;

		[MyCmpReq]
		private ElementConsumer _elementConsumer;

		protected override void OnSpawn()
		{
			base.OnSpawn();

			smi.Get<KBatchedAnimController>().randomiseLoopedOffset = true;
			smi.master._elementEmitter.SetEmitting(false);
			smi.master._elementConsumer.EnableConsumption(false);

			smi.StartSM();
		}

		protected void DestroySelf(object callbackParam)
		{
			CreatureHelpers.DeselectCreature(gameObject);
			Util.KDestroyGameObject(gameObject);
		}

		public Notification CreateDeathNotification()
		{
			return new Notification(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, HashedString.Invalid, (notificationList, data) =>
					 CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), "/t• " + gameObject.GetProperName());
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, PalmeraTree, object>.GameInstance
		{
			public StatesInstance(PalmeraTree master) : base(master) { }

			public bool IsOld() => master._growing.PercentOldAge() > 0.5;
		}

		public class States : GameStateMachine<States, StatesInstance, PalmeraTree>
		{
			public AliveStates Alive;
			public State Dead;

			public override void InitializeStates(out BaseState defaultState)
			{
				serializable = true;
				defaultState = Alive;

				var dead = CREATURES.STATUSITEMS.DEAD.NAME;
				var tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
				var main = Db.Get().StatusItemCategories.Main;

				Dead
					.ToggleStatusItem(dead, tooltip, string.Empty, StatusItem.IconType.Info, 0, false, OverlayModes.None.ID, 0, category: main)
					.Enter(smi =>
					{
						if (smi.master._growing.Replanted && !UprootedMonitor.IsObjectUprooted(masterTarget.Get(smi)))
						{
							smi.master.gameObject.AddOrGet<Notifier>().Add(smi.master.CreateDeathNotification(), string.Empty);
						}

						GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(),
							Grid.SceneLayer.FXFront).SetActive(true);
						smi.master.Trigger((int)GameHashes.Died);
						smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
						Destroy(smi.master.GetComponent<KBatchedAnimController>());
						smi.Schedule(0.5f, smi.master.DestroySelf);
					});

				Alive
					.InitializeStates(masterTarget, Dead)
					.DefaultState(Alive.SeedGrow).ToggleComponent<Growing>();

				Alive.SeedGrow
					.QueueAnim("seed_grow")
					.OnAnimQueueComplete(Alive.Idle);

				Alive.Idle
					.Enter(smi => smi.master._elementConsumer.EnableConsumption(true))
					.EventTransition(GameHashes.Wilt, Alive.WiltingPre, smi => smi.master._wiltCondition.IsWilting())
					.EventTransition(GameHashes.Grow, Alive.PreFruiting, smi => smi.master._growing.ReachedNextHarvest())
					.PlayAnim("idle_loop", KAnim.PlayMode.Loop)
					.Exit(smi => smi.master._elementConsumer.EnableConsumption(false));

				Alive.PreFruiting
					.PlayAnim("grow", KAnim.PlayMode.Once)
					.EventTransition(GameHashes.AnimQueueComplete, Alive.Fruiting);

				Alive.FruitingLost
					.Enter(smi => smi.master._harvestable.SetCanBeHarvested(false))
					.GoTo(Alive.Idle);

				Alive.WiltingPre
					.QueueAnim("wilt_pre")
					.OnAnimQueueComplete(Alive.Wilting);

				Alive.Wilting
					.PlayAnim("idle_wilt_loop", KAnim.PlayMode.Loop)
					.EventTransition(GameHashes.WiltRecover, Alive.Idle, smi => !smi.master._wiltCondition.IsWilting())
					.EventTransition(GameHashes.Harvest, Alive.Harvest);

				Alive.Fruiting
					.DefaultState(Alive.Fruiting.FruitingIdle)
					.EventTransition(GameHashes.Wilt, Alive.WiltingPre)
					.EventTransition(GameHashes.Harvest, Alive.Harvest)
					.EventTransition(GameHashes.Grow, Alive.FruitingLost, smi => !smi.master._growing.ReachedNextHarvest());

				Alive.Fruiting.FruitingIdle
					.PlayAnim("idle_bloom_loop", KAnim.PlayMode.Loop)
					.Enter(smi => smi.master._harvestable.SetCanBeHarvested(true))
					.Enter(smi => smi.master._elementConsumer.EnableConsumption(true))
					.Enter(smi => smi.master._elementEmitter.SetEmitting(true))
					.Update("fruiting_idle", (smi, dt) =>
					{
						if (!smi.IsOld())
							return;
						smi.GoTo(Alive.Fruiting.FruitingOld);
					}, UpdateRate.SIM_4000ms)
					.Exit(smi => smi.master._elementEmitter.SetEmitting(false))
					.Exit(smi => smi.master._elementConsumer.EnableConsumption(false));

				Alive.Fruiting.FruitingOld
					.PlayAnim("wilt", KAnim.PlayMode.Loop)
					.Enter(smi => smi.master._harvestable.SetCanBeHarvested(true))
					.Update("fruiting_old", (smi, dt) =>
					{
						if (smi.IsOld())
							return;
						smi.GoTo(Alive.Fruiting.FruitingIdle);
					}, UpdateRate.SIM_4000ms);

				Alive.Harvest
					.PlayAnim("harvest", KAnim.PlayMode.Once)
					.Enter(smi =>
					{
						if (GameScheduler.Instance == null || smi.master == null) return;

						GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, smi.master._crop.SpawnFruit);
						smi.master._harvestable.SetCanBeHarvested(false);
					})
					.OnAnimQueueComplete(Alive.Idle);
			}

			public class AliveStates : PlantAliveSubState
			{
				public State Idle;
				public State SeedGrow;
				public State PreFruiting;
				public State FruitingLost;
				public State Wilting;
				public State WiltingPre;
				public State Harvest;
				public FruitingState Fruiting;
			}

			public class FruitingState : State
			{
				public State FruitingIdle;
				public State FruitingOld;
			}
		}
	}
}
