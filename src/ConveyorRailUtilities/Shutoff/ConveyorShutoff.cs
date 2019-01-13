namespace ConveyorRailUtilities.Shutoff
{
	public class ConveyorShutoff : KMonoBehaviour
	{
		private int _inputCell = -1;
		private int _outputCell = -1;

		[MyCmpReq]
		private Operational _operational;

		protected override void OnSpawn()
		{
			base.OnSpawn();

			var component = GetComponent<Building>();
			_inputCell = component.GetUtilityInputCell();
			_outputCell = component.GetUtilityOutputCell();

			SolidConduit.GetFlowManager().AddConduitUpdater(ConduitUpdate, ConduitFlowPriority.Default);
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
			    !flowManager.IsConduitFull(_inputCell) || !flowManager.IsConduitEmpty(_outputCell))
				return;

			var pickupable = flowManager.RemovePickupable(_inputCell);
			if (!(bool)  pickupable)
				return;

			flowManager.AddPickupable(_outputCell, pickupable);

			_operational.SetActive(false);
		}
	}
}
