using UnityEngine;

namespace ConveyorShutoff
{
	public class SolidConduitShutoff : KMonoBehaviour
	{
		private int inputCell = -1;
		private int outputCell = -1;

		[MyCmpReq]
		private Operational operational;


		protected override void OnSpawn()
		{
			base.OnSpawn();
			Building component = this.GetComponent<Building>();
			this.inputCell = component.GetUtilityInputCell();
			this.outputCell = component.GetUtilityOutputCell();

			SolidConduit.GetFlowManager().AddConduitUpdater(new System.Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		}

		protected override void OnCleanUp()
		{
			SolidConduit.GetFlowManager().RemoveConduitUpdater(new System.Action<float>(this.ConduitUpdate));
			base.OnCleanUp();
		}

		private void ConduitUpdate(float dt)
		{
			if (this.operational.IsOperational)
			{
				SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
				if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell) ||
				    !flowManager.IsConduitFull(this.inputCell) || !flowManager.IsConduitEmpty(this.outputCell))
					return;

				Pickupable pickupable = flowManager.RemovePickupable(this.inputCell);
				if (!(bool) ((UnityEngine.Object) pickupable))
					return;

				flowManager.AddPickupable(this.outputCell, pickupable);

				this.operational.SetActive(false);
			}
		}
	}
}