using STRINGS;

namespace PalmeraTree
{
	public class PalmeraTree : StateMachineComponent<PalmeraTree.StatesInstance>
	{
		[MyCmpReq]
		private Crop crop;

		[MyCmpReq]
		private WiltCondition wiltCondition;

		[MyCmpReq]
		private Growing growing;

        [MyCmpReq]
        private ReceptacleMonitor rm;

        [MyCmpReq]
		private Harvestable harvestable;

		[MyCmpReq]
		private ElementEmitter elementEmitter;

		[MyCmpReq]
		private ElementConsumer elementConsumer;

		protected override void OnSpawn()
		{
			base.OnSpawn();

			smi.Get<KBatchedAnimController>().randomiseLoopedOffset = true;
			smi.master.elementEmitter.SetEmitting(false);
			smi.master.elementConsumer.EnableConsumption(false);

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

			public bool IsOld() => master.growing.PercentOldAge() > 0.5;
		}

        public class AnimSet
        {
            public const string grow = nameof(grow);
            public const string grow_pst = nameof(grow_pst);
            public const string idle_full = nameof(idle_full);
            public const string wilt_pre = nameof(wilt_pre);
            public const string wilt = nameof(wilt);
            public const string wilt_pst = nameof(wilt_pst);
            public const string harvest = nameof(harvest);
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
                        if (smi.master.rm.Replanted && !smi.master.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
                            smi.master.gameObject.AddOrGet<Notifier>().Add(smi.master.CreateDeathNotification());

                        GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
                        if (smi.master.harvestable != null && smi.master.harvestable.CanBeHarvested && GameScheduler.Instance != null)
                            GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, smi.master.crop.SpawnFruit);

                        smi.master.Trigger((int)GameHashes.Died);
						smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
						Destroy(smi.master.GetComponent<KBatchedAnimController>());
						smi.Schedule(0.5f, smi.master.DestroySelf);
                    });

				Alive
					.InitializeStates(masterTarget, Dead)
					.DefaultState(Alive.Idle)
                    .ToggleComponent<Growing>();


				Alive.Idle
					.Enter(smi => smi.master.elementConsumer.EnableConsumption(true))
					.EventTransition(GameHashes.Wilt, Alive.WiltingPre, smi => smi.master.wiltCondition.IsWilting())
					.EventTransition(GameHashes.Grow, Alive.PreFruiting, smi => smi.master.growing.ReachedNextHarvest())
					.PlayAnim(AnimSet.grow, KAnim.PlayMode.Loop)
					.Exit(smi => smi.master.elementConsumer.EnableConsumption(false));

				Alive.PreFruiting
					.PlayAnim("grow", KAnim.PlayMode.Once)
					.EventTransition(GameHashes.AnimQueueComplete, Alive.Fruiting);

				Alive.FruitingLost
					.Enter(smi => smi.master.harvestable.SetCanBeHarvested(false))
					.GoTo(Alive.Idle);

				Alive.WiltingPre
					.QueueAnim(AnimSet.wilt_pre)
					.OnAnimQueueComplete(Alive.Wilting);

				Alive.Wilting
					.PlayAnim(AnimSet.wilt, KAnim.PlayMode.Loop)
					.EventTransition(GameHashes.WiltRecover, Alive.WiltingPst, smi => !smi.master.wiltCondition.IsWilting())
					.EventTransition(GameHashes.Harvest, Alive.Harvest);

                Alive.WiltingPst
                    .PlayAnim(AnimSet.wilt, KAnim.PlayMode.Once)
                    .OnAnimQueueComplete(Alive.Idle);

                Alive.Fruiting
					.DefaultState(Alive.Fruiting.FruitingIdle)
					.EventTransition(GameHashes.Wilt, Alive.WiltingPre)
					.EventTransition(GameHashes.Harvest, Alive.Harvest)
					.EventTransition(GameHashes.Grow, Alive.FruitingLost, smi => !smi.master.growing.ReachedNextHarvest());

				Alive.Fruiting.FruitingIdle
					.PlayAnim(AnimSet.idle_full, KAnim.PlayMode.Loop)
					.Enter(smi => smi.master.harvestable.SetCanBeHarvested(true))
					.Enter(smi => smi.master.elementConsumer.EnableConsumption(true))
					.Enter(smi => smi.master.elementEmitter.SetEmitting(true))
					.Update("fruiting_idle", (smi, dt) =>
					{
						if (!smi.IsOld())
							return;
						smi.GoTo(Alive.Fruiting.FruitingOld);
					}, UpdateRate.SIM_4000ms)
					.Exit(smi => smi.master.elementEmitter.SetEmitting(false))
					.Exit(smi => smi.master.elementConsumer.EnableConsumption(false));

				Alive.Fruiting.FruitingOld
					.PlayAnim(AnimSet.wilt, KAnim.PlayMode.Loop)
					.Enter(smi => smi.master.harvestable.SetCanBeHarvested(true))
					.Update("fruiting_old", (smi, dt) =>
					{
						if (smi.IsOld())
							return;
						smi.GoTo(Alive.Fruiting.FruitingIdle);
					}, UpdateRate.SIM_4000ms);

				Alive.Harvest
					.PlayAnim(AnimSet.harvest, KAnim.PlayMode.Once)
					.Enter(smi =>
					{
						if (GameScheduler.Instance == null || smi.master == null) return;

						GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, smi.master.crop.SpawnFruit);
						smi.master.harvestable.SetCanBeHarvested(false);
					})
					.OnAnimQueueComplete(Alive.Idle);
			}

			public class AliveStates : PlantAliveSubState
			{
				public State Idle;
				public State PreFruiting;
				public State FruitingLost;
                public State WiltingPre;
                public State Wilting;
                public State WiltingPst;
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
