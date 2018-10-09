using System;
using System.Collections.Generic;
using Harmony;
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
			accumulator = Game.Instance.accumulators.Add("Flow", (KMonoBehaviour)this);
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();
			Building component = GetComponent<Building>();
			inputCell = component.GetUtilityInputCell();
			outputCell = component.GetUtilityOutputCell();

			secondaryOutputCell = Grid.OffsetCell(Grid.PosToCell(transform.GetPosition()), component.GetRotatedOffset(SecondaryPort.offset));
			var secondOutput = new FlowUtilityNetwork.NetworkItem(SecondaryPort.conduitType, Endpoint.Source, secondaryOutputCell, gameObject);

			IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(SecondaryPort.conduitType);
			networkManager.AddToNetworks(secondaryOutputCell, (object)secondOutput, true);

			Conduit.GetFlowManager(type).AddConduitUpdater(ConduitUpdate);
		}

		protected override void OnCleanUp()
		{
			Conduit.GetFlowManager(type).RemoveConduitUpdater(ConduitUpdate);
			Game.Instance.accumulators.Remove(accumulator);
			base.OnCleanUp();
		}

		public bool IsOperational
		{
			get
			{
				ConduitFlow flowManager = Conduit.GetFlowManager(type);
				return flowManager.HasConduit(outputCell) || flowManager.HasConduit(secondaryOutputCell);
			}
		}

		private void ConduitUpdate(float dt)
		{
			ConduitFlow flowManager = Conduit.GetFlowManager(type);
			if (!flowManager.HasConduit(inputCell))
				return;

			if (IsOperational)
			{
				ConduitFlow.ConduitContents contents = flowManager.GetContents(inputCell);
				if (contents.mass <= 0.0)
					return;

				ConduitFlow.ConduitContents contentOutput1 = flowManager.GetContents(outputCell);
				ConduitFlow.ConduitContents contentOutput2 = flowManager.GetContents(secondaryOutputCell);

				var maxMass = Traverse.Create(flowManager).Field("MaxMass").GetValue<float>();  //type == ConduitType.Liquid ? 10f : 1f;
				var halfMass = contents.mass / 2f;

				var willFitInOutput1 = maxMass - contentOutput1.mass;
				var willFitInOutput2 = maxMass - contentOutput2.mass;

				float delta1 = 0;
				float delta2 = 0;

				if (Math.Abs(willFitInOutput1) < 0.001f && Math.Abs(willFitInOutput2) < 0.001f)
				{
					//do nothing
				}
				else if (!flowManager.HasConduit(secondaryOutputCell))
				{
					delta1 = flowManager.AddElement(outputCell, contents.element, contents.mass, contents.temperature,
						contents.diseaseIdx, contents.diseaseCount);
				}
				else if (!flowManager.HasConduit(outputCell))
				{
					delta2 = flowManager.AddElement(secondaryOutputCell, contents.element, contents.mass, contents.temperature,
						contents.diseaseIdx, contents.diseaseCount);
				}
				else if (willFitInOutput1 >= halfMass && willFitInOutput2 >= halfMass)
				{
					delta1 = flowManager.AddElement(outputCell, contents.element, halfMass, contents.temperature,
						contents.diseaseIdx, contents.diseaseCount / 2);
					delta2 = flowManager.AddElement(secondaryOutputCell, contents.element, halfMass, contents.temperature,
						contents.diseaseIdx, contents.diseaseCount / 2);
				}
				else if (willFitInOutput1 < halfMass)
				{
					var overflowOutput1 = halfMass - willFitInOutput1;
					var ratio = (halfMass - overflowOutput1) / halfMass;
					delta1 = flowManager.AddElement(outputCell, contents.element, halfMass - overflowOutput1,
						contents.temperature, contents.diseaseIdx, (int)((contents.diseaseCount / 2f) * ratio));
					delta2 = flowManager.AddElement(secondaryOutputCell, contents.element, halfMass + overflowOutput1,
						contents.temperature, contents.diseaseIdx, (int)((contents.diseaseCount / 2f) * (1f / ratio)));
				}
				else
				{
					var overflowOutput2 = halfMass - willFitInOutput2;
					var ratio = (halfMass - overflowOutput2) / halfMass;
					delta1 = flowManager.AddElement(secondaryOutputCell, contents.element, halfMass - overflowOutput2,
						contents.temperature, contents.diseaseIdx, (int)((contents.diseaseCount / 2f) * ratio));
					delta2 = flowManager.AddElement(outputCell, contents.element, halfMass + overflowOutput2,
						contents.temperature, contents.diseaseIdx, (int)((contents.diseaseCount / 2f) * (1f / ratio)));
				}

				flowManager.RemoveElement(inputCell, delta1);
				flowManager.RemoveElement(inputCell, delta2);

				Game.Instance.accumulators.Accumulate(accumulator, contents.mass);
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
			IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(type);
			UtilityNetwork networkForCell1 = networkManager.GetNetworkForCell(inputCell);
			if (networkForCell1 != null)
				networks.Add(networkForCell1);
			UtilityNetwork networkForCell2 = networkManager.GetNetworkForCell(outputCell);
			if (networkForCell2 != null)
				networks.Add(networkForCell2);
			UtilityNetwork networkForCell3 = networkManager.GetNetworkForCell(secondaryOutputCell);
			if (networkForCell3 != null)
				networks.Add(networkForCell3);
		}

		public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
		{
			bool flag = false;
			IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(type);
			return flag || networks.Contains(networkManager.GetNetworkForCell(inputCell)) || networks.Contains(networkManager.GetNetworkForCell(outputCell))
				   || networks.Contains(networkManager.GetNetworkForCell(secondaryOutputCell));
		}

		public int GetNetworkCell()
		{
			return inputCell;
		}
	}
}
