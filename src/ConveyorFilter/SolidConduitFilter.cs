using KSerialization;
using UnityEngine;

namespace ConveyorFilter
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class SolidConduitFilter : KMonoBehaviour, ISecondaryOutput
	{
		[SerializeField]
		public ConduitPortInfo SecondaryPort;

		private int inputCell = -1;
		private int outputCell = -1;
		private int filteredCell = -1;

		[MyCmpReq]
		private TreeFilterable treeFilterable;

		[MyCmpReq]
		private Operational operational;


		protected override void OnSpawn()
		{
			base.OnSpawn();
			Building component = this.GetComponent<Building>();
			this.inputCell = component.GetUtilityInputCell();
			this.outputCell = component.GetUtilityOutputCell();

			this.filteredCell = Grid.OffsetCell(Grid.PosToCell(transform.GetPosition()), component.GetRotatedOffset(SecondaryPort.offset));		
			var itemFilter = new FlowUtilityNetwork.NetworkItem(this.SecondaryPort.conduitType, Endpoint.Source, this.filteredCell, this.gameObject);
			Game.Instance.solidConduitSystem.AddToNetworks(filteredCell,(object) itemFilter, true);

			SolidConduit.GetFlowManager().AddConduitUpdater(new System.Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);		
		}

		protected override void OnCleanUp()
		{
			SolidConduit.GetFlowManager().RemoveConduitUpdater(new System.Action<float>(this.ConduitUpdate));
			base.OnCleanUp();
		}

		private void ConduitUpdate(float dt)
		{
			bool flag = false;
			if (this.operational.IsOperational)
			{
				SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
				if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell) ||
				    !flowManager.HasConduit(this.filteredCell) || (!flowManager.IsConduitFull(this.inputCell) ||
				                                                   !flowManager.IsConduitEmpty(this.outputCell) ||
				                                                   !flowManager.IsConduitEmpty(this.filteredCell)))
					return;

				var acceptedTags = treeFilterable.AcceptedTags;

				Pickupable pickupable = flowManager.RemovePickupable(this.inputCell);
				if (!(bool) ((UnityEngine.Object) pickupable))
					return;

				foreach (var acceptedTag in acceptedTags)
				{
					if (pickupable.HasTag(acceptedTag))
					{
						flowManager.AddPickupable(this.filteredCell, pickupable);
						return;
					}
				}

				flowManager.AddPickupable(this.outputCell, pickupable);
				this.operational.SetActive(flag, false);
			}
		}

		public ConduitType GetSecondaryConduitType()
		{
			return SecondaryPort.conduitType;
		}

		public CellOffset GetSecondaryConduitOffset()
		{
			return SecondaryPort.offset;
		}
	}
}