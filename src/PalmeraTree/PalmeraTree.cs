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
			public StatesInstance(PalmeraTree master)
				: base(master)
			{
			}

			public bool IsOld()
			{
				return master.growing.PercentOldAge() > 0.5;
			}
		}

		public class States : GameStateMachine<States, StatesInstance, PalmeraTree>
		{
			public AliveStates alive;
			public State dead;

			public override void InitializeStates(out BaseState default_state)
			{
				serializable = true;
				default_state = alive;

				string plantname = CREATURES.STATUSITEMS.DEAD.NAME;
				string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
				StatusItemCategory main = Db.Get().StatusItemCategories.Main;

				dead.ToggleStatusItem(plantname, tooltip, string.Empty, StatusItem.IconType.Info, 0, false, OverlayModes.None.ID, 0, null,
						null, main)
					.Enter(smi =>
					{
						if (smi.master.growing.Replanted && !UprootedMonitor.IsObjectUprooted(masterTarget.Get(smi)))
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

				alive
					.InitializeStates(masterTarget, dead)
					.DefaultState(alive.seed_grow).ToggleComponent<Growing>();

				alive.seed_grow
					.QueueAnim("seed_grow")
					.OnAnimQueueComplete(alive.idle);

				alive.idle
					.Enter(smi => smi.master.elementConsumer.EnableConsumption(true))
					.EventTransition(GameHashes.Wilt, alive.wilting_pre, smi => smi.master.wiltCondition.IsWilting())
					.EventTransition(GameHashes.Grow, alive.pre_fruiting, smi => smi.master.growing.ReachedNextHarvest())
					.PlayAnim("idle_loop", KAnim.PlayMode.Loop)
					.Exit(smi => smi.master.elementConsumer.EnableConsumption(false));

				alive.pre_fruiting
					.PlayAnim("grow", KAnim.PlayMode.Once)
					.EventTransition(GameHashes.AnimQueueComplete, alive.fruiting);

				alive.fruiting_lost
					.Enter(smi => smi.master.harvestable.SetCanBeHarvested(false))
					.GoTo(alive.idle);

				alive.wilting_pre
					.QueueAnim("wilt_pre")
					.OnAnimQueueComplete(alive.wilting);

				alive.wilting
					.PlayAnim("idle_wilt_loop", KAnim.PlayMode.Loop)
					.EventTransition(GameHashes.WiltRecover, alive.idle, smi => !smi.master.wiltCondition.IsWilting())
					.EventTransition(GameHashes.Harvest, alive.harvest);


				alive.fruiting
					.DefaultState(alive.fruiting.fruiting_idle)
					.EventTransition(GameHashes.Wilt, alive.wilting_pre)
					.EventTransition(GameHashes.Harvest, alive.harvest)
					.EventTransition(GameHashes.Grow, alive.fruiting_lost, smi => !smi.master.growing.ReachedNextHarvest());

				alive.fruiting.fruiting_idle.PlayAnim("idle_bloom_loop", KAnim.PlayMode.Loop)
					.Enter(smi => smi.master.harvestable.SetCanBeHarvested(true))
					.Enter(smi => smi.master.elementConsumer.EnableConsumption(true))
					.Enter(smi => smi.master.elementEmitter.SetEmitting(true))
					.Update("fruiting_idle", (smi, dt) =>
					{
						if (!smi.IsOld())
							return;
						smi.GoTo(alive.fruiting.fruiting_old);
					}, UpdateRate.SIM_4000ms)
					.Exit(smi => smi.master.elementEmitter.SetEmitting(false))
					.Exit(smi => smi.master.elementConsumer.EnableConsumption(false));

				alive.fruiting.fruiting_old
					.PlayAnim("wilt", KAnim.PlayMode.Loop)
					.Enter(smi => smi.master.harvestable.SetCanBeHarvested(true))
					.Update("fruiting_old", (smi, dt) =>
					{
						if (smi.IsOld())
							return;
						smi.GoTo(alive.fruiting.fruiting_idle);
					}, UpdateRate.SIM_4000ms);

				alive.harvest
					.PlayAnim("harvest", KAnim.PlayMode.Once)
					.Enter(smi =>
					{
						if (GameScheduler.Instance != null && smi.master != null)
							GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, smi.master.crop.SpawnFruit);
						smi.master.harvestable.SetCanBeHarvested(false);
					})
					.OnAnimQueueComplete(alive.idle);
			}

			public class AliveStates : PlantAliveSubState
			{
				public State idle;
				public State seed_grow;
				public State pre_fruiting;
				public State fruiting_lost;
				public State barren;
				public State wilting;
				public State wilting_pre;
				public State wilting_pst;
				public State destroy;
				public State harvest;
				public FruitingState fruiting;
			}

			public class FruitingState : State
			{
				public State fruiting_idle;
				public State fruiting_old;
			}
		}
	}
}