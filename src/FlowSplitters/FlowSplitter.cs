using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlowSplitters
{
	public class FlowSplitter : KMonoBehaviour, IBridgedNetworkItem, ISecondaryOutput
	{
		[SerializeField]
		public ConduitPortInfo SecondaryPort;

		[SerializeField]
		public ConduitType type;

		[MyCmpReq]
		private Operational operational;

		private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;
		private int inputCell;
		private int outputCell;
		private int secondaryOutputCell;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.accumulator = Game.Instance.accumulators.Add("Flow", (KMonoBehaviour)this);
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();
			Building component = this.GetComponent<Building>();
			this.inputCell = component.GetUtilityInputCell();
			this.outputCell = component.GetUtilityOutputCell();

			this.secondaryOutputCell = Grid.OffsetCell(Grid.PosToCell(transform.GetPosition()), component.GetRotatedOffset(SecondaryPort.offset));
			var secondOutput = new FlowUtilityNetwork.NetworkItem(this.SecondaryPort.conduitType, Endpoint.Source, this.secondaryOutputCell, this.gameObject);

			IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(SecondaryPort.conduitType);
			networkManager.AddToNetworks(secondaryOutputCell, (object)secondOutput, true);

			Conduit.GetFlowManager(this.type).AddConduitUpdater(new System.Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		}

		protected override void OnCleanUp()
		{
			Conduit.GetFlowManager(this.type).RemoveConduitUpdater(new System.Action<float>(this.ConduitUpdate));
			Game.Instance.accumulators.Remove(this.accumulator);
			base.OnCleanUp();
		}

		private void ConduitUpdate(float dt)
		{
			ConduitFlow flowManager = Conduit.GetFlowManager(this.type);
			if (!flowManager.HasConduit(this.inputCell))
				return;

			bool flag = false;
			if (this.operational.IsOperational)
			{
				ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
				if ((double)contents.mass <= 0.0)
					return;

				ConduitFlow.ConduitContents contentOutput1 = flowManager.GetContents(this.outputCell);
				ConduitFlow.ConduitContents contentOutput2 = flowManager.GetContents(this.secondaryOutputCell);

				var maxMass = this.type == ConduitType.Liquid ? 10f : 1f;
				var halfMass = contents.mass / 2f;

				var willFitInOutput1 = maxMass - contentOutput1.mass;
				var willFitInOutput2 = maxMass - contentOutput2.mass;

				float delta1 = 0;
				float delta2 = 0;

				
				if (!flowManager.HasConduit(this.secondaryOutputCell))
				{
					delta1 = flowManager.AddElement(this.outputCell, contents.element, contents.mass, contents.temperature,
						contents.diseaseIdx, contents.diseaseCount);
				}
				else if (willFitInOutput1 >= halfMass && willFitInOutput2 >= halfMass)
				{
					delta1 = flowManager.AddElement(this.outputCell, contents.element, halfMass, contents.temperature,
						contents.diseaseIdx, contents.diseaseCount / 2);
					delta2 = flowManager.AddElement(this.secondaryOutputCell, contents.element, halfMass, contents.temperature,
						contents.diseaseIdx, contents.diseaseCount / 2);
				}
				else if (willFitInOutput1 < halfMass)
				{
					var overflowOutput1 = halfMass - willFitInOutput1;
					var ratio = (halfMass - overflowOutput1) / halfMass;
					delta1 = flowManager.AddElement(this.outputCell, contents.element, halfMass - overflowOutput1,
						contents.temperature, contents.diseaseIdx, (int)((contents.diseaseCount / 2f) * ratio));
					delta2 = flowManager.AddElement(this.secondaryOutputCell, contents.element, halfMass + overflowOutput1,
						contents.temperature, contents.diseaseIdx, (int)((contents.diseaseCount / 2f) * (1f / ratio)));
				}
				else
				{
					var overflowOutput2 = halfMass - willFitInOutput2;
					var ratio = (halfMass - overflowOutput2) / halfMass;
					delta1 = flowManager.AddElement(this.secondaryOutputCell, contents.element, halfMass - overflowOutput2,
						contents.temperature, contents.diseaseIdx, (int)((contents.diseaseCount / 2f) * ratio));
					delta2 = flowManager.AddElement(this.outputCell, contents.element, halfMass + overflowOutput2,
						contents.temperature, contents.diseaseIdx, (int)((contents.diseaseCount / 2f) * (1f / ratio)));
				}

				flowManager.RemoveElement(this.inputCell, delta1);
				flowManager.RemoveElement(this.inputCell, delta2);

				Game.Instance.accumulators.Accumulate(this.accumulator, contents.mass);
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

		public void AddNetworks(ICollection<UtilityNetwork> networks)
		{
			IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.type);
			UtilityNetwork networkForCell1 = networkManager.GetNetworkForCell(this.inputCell);
			if (networkForCell1 != null)
				networks.Add(networkForCell1);
			UtilityNetwork networkForCell2 = networkManager.GetNetworkForCell(this.outputCell);
			if (networkForCell2 != null)
				networks.Add(networkForCell2);
			UtilityNetwork networkForCell3 = networkManager.GetNetworkForCell(this.secondaryOutputCell);
			if (networkForCell3 != null)
				networks.Add(networkForCell3);
		}

		public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
		{
			bool flag = false;
			IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.type);
			return flag || networks.Contains(networkManager.GetNetworkForCell(this.inputCell)) || networks.Contains(networkManager.GetNetworkForCell(this.outputCell))
				   || networks.Contains(networkManager.GetNetworkForCell(this.secondaryOutputCell));
		}

		public int GetNetworkCell()
		{
			return this.inputCell;
		}
	}
}
