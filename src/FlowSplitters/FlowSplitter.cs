using System;
using System.Collections.Generic;
using UnityEngine;

namespace FlowSplitters
{
	public class FlowSplitter : KMonoBehaviour, IBridgedNetworkItem, ISecondaryOutput
	{
		[SerializeField]
		public ConduitPortInfo SecondaryPort;

		[SerializeField]
		public ConduitType Type;

		private HandleVector<int>.Handle _accumulator = HandleVector<int>.InvalidHandle;
		private int _inputCell;
		private int _outputCell;
		private int _secondaryOutputCell;
		private FlowUtilityNetwork.NetworkItem _secondOutputItem;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			_accumulator = Game.Instance.accumulators.Add("Flow", this);
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();

			var building = GetComponent<Building>();
			_inputCell = building.GetUtilityInputCell();
			_outputCell = building.GetUtilityOutputCell();

			_secondaryOutputCell = Grid.OffsetCell(Grid.PosToCell(transform.GetPosition()), building.GetRotatedOffset(SecondaryPort.offset));
			_secondOutputItem = new FlowUtilityNetwork.NetworkItem(SecondaryPort.conduitType, Endpoint.Source, _secondaryOutputCell, gameObject);

			var networkManager = Conduit.GetNetworkManager(SecondaryPort.conduitType);
			networkManager.AddToNetworks(_secondaryOutputCell, _secondOutputItem, true);

			Conduit.GetFlowManager(Type).AddConduitUpdater(ConduitUpdate);
		}

		protected override void OnCleanUp()
		{
			Conduit.GetNetworkManager(Type).RemoveFromNetworks(_secondaryOutputCell, _secondOutputItem, true);
			Conduit.GetFlowManager(Type).RemoveConduitUpdater(ConduitUpdate);
			Game.Instance.accumulators.Remove(_accumulator);
			base.OnCleanUp();
		}

		public bool IsOperational
		{
			get
			{
				var flowManager = Conduit.GetFlowManager(Type);
				return flowManager.HasConduit(_outputCell) || flowManager.HasConduit(_secondaryOutputCell);
			}
		}

		private void ConduitUpdate(float dt)
		{
			var flowManager = Conduit.GetFlowManager(Type);
			if (!flowManager.HasConduit(_inputCell) || !IsOperational)
				return;

			var contents = flowManager.GetContents(_inputCell);
			if (contents.mass <= 0.0)
				return;

			var contentOutput1 = flowManager.GetContents(_outputCell);
			var contentOutput2 = flowManager.GetContents(_secondaryOutputCell);

			var maxMass = Type == ConduitType.Liquid ? 10f : 1f;
			var halfMass = contents.mass / 2f;

			var willFitInOutput1 = maxMass - contentOutput1.mass;
			var willFitInOutput2 = maxMass - contentOutput2.mass;

			float delta1 = 0;
			float delta2 = 0;

			if (Math.Abs(willFitInOutput1) < 0.001f && Math.Abs(willFitInOutput2) < 0.001f)
			{
				//do nothing
			}
			else if (!flowManager.HasConduit(_secondaryOutputCell))
			{
				delta1 = flowManager.AddElement(_outputCell, contents.element, contents.mass, contents.temperature,
					contents.diseaseIdx, contents.diseaseCount);
			}
			else if (!flowManager.HasConduit(_outputCell))
			{
				delta2 = flowManager.AddElement(_secondaryOutputCell, contents.element, contents.mass, contents.temperature,
					contents.diseaseIdx, contents.diseaseCount);
			}
			else if (willFitInOutput1 >= halfMass && willFitInOutput2 >= halfMass)
			{
				delta1 = flowManager.AddElement(_outputCell, contents.element, halfMass, contents.temperature,
					contents.diseaseIdx, contents.diseaseCount / 2);
				delta2 = flowManager.AddElement(_secondaryOutputCell, contents.element, halfMass, contents.temperature,
					contents.diseaseIdx, contents.diseaseCount / 2);
			}
			else if (willFitInOutput1 < halfMass)
			{
				var overflowOutput1 = halfMass - willFitInOutput1;
				var ratio = (halfMass - overflowOutput1) / halfMass;
				delta1 = flowManager.AddElement(_outputCell, contents.element, halfMass - overflowOutput1,
					contents.temperature, contents.diseaseIdx, (int)((contents.diseaseCount / 2f) * ratio));
				delta2 = flowManager.AddElement(_secondaryOutputCell, contents.element, halfMass + overflowOutput1,
					contents.temperature, contents.diseaseIdx, (int)((contents.diseaseCount / 2f) * (1f / ratio)));
			}
			else
			{
				var overflowOutput2 = halfMass - willFitInOutput2;
				var ratio = (halfMass - overflowOutput2) / halfMass;
				delta1 = flowManager.AddElement(_secondaryOutputCell, contents.element, halfMass - overflowOutput2,
					contents.temperature, contents.diseaseIdx, (int)((contents.diseaseCount / 2f) * ratio));
				delta2 = flowManager.AddElement(_outputCell, contents.element, halfMass + overflowOutput2,
					contents.temperature, contents.diseaseIdx, (int)((contents.diseaseCount / 2f) * (1f / ratio)));
			}

			flowManager.RemoveElement(_inputCell, delta1);
			flowManager.RemoveElement(_inputCell, delta2);

			Game.Instance.accumulators.Accumulate(_accumulator, contents.mass);
		}

		public ConduitType GetSecondaryConduitType() => SecondaryPort.conduitType;

		public CellOffset GetSecondaryConduitOffset() => SecondaryPort.offset;

		public void AddNetworks(ICollection<UtilityNetwork> networks)
		{
			var networkManager = Conduit.GetNetworkManager(Type);

			var networkForCell1 = networkManager.GetNetworkForCell(_inputCell);
			if (networkForCell1 != null)
				networks.Add(networkForCell1);

			var networkForCell2 = networkManager.GetNetworkForCell(_outputCell);
			if (networkForCell2 != null)
				networks.Add(networkForCell2);

			var networkForCell3 = networkManager.GetNetworkForCell(_secondaryOutputCell);
			if (networkForCell3 != null)
				networks.Add(networkForCell3);
		}

		public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
		{
			var networkManager = Conduit.GetNetworkManager(Type);
			return networks.Contains(networkManager.GetNetworkForCell(_inputCell)) || networks.Contains(networkManager.GetNetworkForCell(_outputCell))
				   || networks.Contains(networkManager.GetNetworkForCell(_secondaryOutputCell));
		}

		public int GetNetworkCell()
		{
			return _inputCell;
		}
	}
}
