using System;
using System.Collections.Generic;
using STRINGS;
using Object = UnityEngine.Object;

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
		private KAnimControllerBase animController;
		[MyCmpReq]
		private ElementEmitter elementEmitter;

		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.smi.Get<KBatchedAnimController>().randomiseLoopedOffset = true;
			this.smi.master.elementEmitter.SetEmitting(false);
			this.smi.StartSM();
		}

		protected void DestroySelf(object callbackParam)
		{
			CreatureHelpers.DeselectCreature(this.gameObject);
			Util.KDestroyGameObject(this.gameObject);
		}

		public Notification CreateDeathNotification()
		{
			return new Notification((string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad,
				HashedString.Invalid,
				(Func<List<Notification>, object, string>) ((notificationList, data) =>
					(string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false)),
				(object) ("/t• " + this.gameObject.GetProperName()), true, 0.0f, (Notification.ClickCallback) null, (object) null);
		}

		public class StatesInstance : GameStateMachine<PalmeraTree.States, PalmeraTree.StatesInstance, PalmeraTree, object>.
			GameInstance
		{
			public StatesInstance(PalmeraTree master)
				: base(master)
			{
			}

			public bool IsOld()
			{
				return (double) this.master.growing.PercentOldAge() > 0.5;
			}
		}

		public class States : GameStateMachine<PalmeraTree.States, PalmeraTree.StatesInstance, PalmeraTree>
		{
			public PalmeraTree.States.AliveStates alive;
			public GameStateMachine<PalmeraTree.States, PalmeraTree.StatesInstance, PalmeraTree, object>.State dead;

			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				base.serializable = true;
				default_state = this.alive;

				string plantname = CREATURES.STATUSITEMS.DEAD.NAME;
				string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
				StatusItemCategory main = Db.Get().StatusItemCategories.Main;

				dead.ToggleStatusItem(plantname, tooltip, string.Empty, StatusItem.IconType.Info, 0, false, SimViewMode.None, 0, null,
						null, main)
					.Enter(smi =>
					{
						if (smi.master.growing.Replanted && !UprootedMonitor.IsObjectUprooted(this.masterTarget.Get(smi)))
						{
							smi.master.gameObject.AddOrGet<Notifier>().Add(smi.master.CreateDeathNotification(), string.Empty);
						}

						GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(),
							Grid.SceneLayer.FXFront).SetActive(true);
						smi.master.Trigger((int) GameHashes.Died, null);
						smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
						Destroy(smi.master.GetComponent<KBatchedAnimController>());
						smi.Schedule(0.5f, smi.master.DestroySelf, null);
					});

				this.alive
					.InitializeStates(this.masterTarget, this.dead)
					.DefaultState(this.alive.seed_grow).ToggleComponent<Growing>();

				this.alive.seed_grow
					.QueueAnim("seed_grow")
					.OnAnimQueueComplete(this.alive.idle);

				this.alive.idle
					.EventTransition(GameHashes.Wilt, this.alive.wilting_pre, smi => smi.master.wiltCondition.IsWilting())
					.EventTransition(GameHashes.Grow, this.alive.pre_fruiting, smi => smi.master.growing.ReachedNextHarvest())
					.PlayAnim("idle_loop", KAnim.PlayMode.Loop);

				this.alive.pre_fruiting
					.PlayAnim("grow", KAnim.PlayMode.Once)
					.EventTransition(GameHashes.AnimQueueComplete, this.alive.fruiting);

				this.alive.fruiting_lost
					.Enter(smi => smi.master.harvestable.SetCanBeHarvested(false))
					.GoTo(this.alive.idle);

				this.alive.wilting_pre
					.QueueAnim("wilt_pre")
					.OnAnimQueueComplete(this.alive.wilting);

				this.alive.wilting
					.PlayAnim("idle_wilt_loop", KAnim.PlayMode.Loop)
					.EventTransition(GameHashes.WiltRecover, this.alive.idle, smi => !smi.master.wiltCondition.IsWilting())
					.EventTransition(GameHashes.Harvest, this.alive.harvest);


				this.alive.fruiting
					.DefaultState(this.alive.fruiting.fruiting_idle)
					.EventTransition(GameHashes.Wilt, this.alive.wilting_pre)
					.EventTransition(GameHashes.Harvest, this.alive.harvest)
					.EventTransition(GameHashes.Grow, this.alive.fruiting_lost, smi => !smi.master.growing.ReachedNextHarvest());

				this.alive.fruiting.fruiting_idle.PlayAnim("idle_bloom_loop", KAnim.PlayMode.Loop)
					.Enter(smi => smi.master.harvestable.SetCanBeHarvested(true))
					.Enter(smi => smi.master.elementEmitter.SetEmitting(true))
					.Update("fruiting_idle", (smi, dt) =>
					{
						if (!smi.IsOld())
							return;
						smi.GoTo(this.alive.fruiting.fruiting_old);
					}, UpdateRate.SIM_4000ms)
					.Exit(smi => smi.master.elementEmitter.SetEmitting(false));

				this.alive.fruiting.fruiting_old
					.PlayAnim("wilt", KAnim.PlayMode.Loop)
					.Enter(smi => smi.master.harvestable.SetCanBeHarvested(true))
					.Update("fruiting_old", (smi, dt) =>
					{
						if (smi.IsOld())
							return;
						smi.GoTo(this.alive.fruiting.fruiting_idle);
					}, UpdateRate.SIM_4000ms);

				this.alive.harvest
					.PlayAnim("harvest", KAnim.PlayMode.Once)
					.Enter(smi =>
					{
						if (GameScheduler.Instance != null && smi.master != null)
							GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, smi.master.crop.SpawnFruit);
						smi.master.harvestable.SetCanBeHarvested(false);
					})
					.OnAnimQueueComplete(this.alive.idle);
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