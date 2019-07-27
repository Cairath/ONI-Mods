using KSerialization;
using UnityEngine;

namespace ConveyorRailUtilities.Filter
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class ConveyorFilter : KMonoBehaviour, ISecondaryOutput
	{
		[SerializeField]
		public ConduitPortInfo SecondaryPort;

		private int _inputCell = -1;
		private int _outputCell = -1;
		private int _filteredCell = -1;

		[MyCmpReq]
		private TreeFilterable _treeFilterable;

		[MyCmpReq]
		private Operational _operational;

		protected override void OnSpawn()
		{
			base.OnSpawn();

			var component = GetComponent<Building>();
			_inputCell = component.GetUtilityInputCell();
			_outputCell = component.GetUtilityOutputCell();

			_filteredCell = Grid.OffsetCell(Grid.PosToCell(transform.GetPosition()), component.GetRotatedOffset(SecondaryPort.offset));	
			
			var itemFilter = new FlowUtilityNetwork.NetworkItem(SecondaryPort.conduitType, Endpoint.Source, _filteredCell, gameObject);

			Game.Instance.solidConduitSystem.AddToNetworks(_filteredCell, itemFilter, true);
			SolidConduit.GetFlowManager().AddConduitUpdater(ConduitUpdate);		
		}

		protected override void OnCleanUp()
		{
			SolidConduit.GetFlowManager().RemoveConduitUpdater(ConduitUpdate);
			base.OnCleanUp();
		}

		private void ConduitUpdate(float dt)
		{
			if (!_operational.IsOperational) return;

			var flowManager = SolidConduit.GetFlowManager();
			if (!flowManager.HasConduit(_inputCell) || !flowManager.HasConduit(_outputCell) ||
			    !flowManager.HasConduit(_filteredCell) || (!flowManager.IsConduitFull(_inputCell) ||
			                                               !flowManager.IsConduitEmpty(_outputCell) ||
			                                               !flowManager.IsConduitEmpty(_filteredCell)))
				return;

			var acceptedTags = _treeFilterable.AcceptedTags;

			var pickupable = flowManager.RemovePickupable(_inputCell);
			if (!(bool) pickupable)
				return;

			foreach (var acceptedTag in acceptedTags)
			{
				if (pickupable.HasTag(acceptedTag))
				{
					flowManager.AddPickupable(_filteredCell, pickupable);
					return;
				}
			}

			flowManager.AddPickupable(_outputCell, pickupable);
			_operational.SetActive(false);
		}

		public ConduitType GetSecondaryConduitType() => SecondaryPort.conduitType;

		public CellOffset GetSecondaryConduitOffset() => SecondaryPort.offset;
	}
}
