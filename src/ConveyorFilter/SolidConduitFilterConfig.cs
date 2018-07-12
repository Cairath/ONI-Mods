using System.Collections.Generic;
using KSerialization;
using TUNING;
using UnityEngine;

namespace ConveyorFilter
{
	public class SolidConduitFilterConfig : IBuildingConfig
	{
		public const string ID = "SolidConduitFilter";
		private ConduitPortInfo secondaryPort = new ConduitPortInfo(ConduitType.Solid, new CellOffset(0, 0));

		public override BuildingDef CreateBuildingDef()
		{
			string id = "SolidConduitFilter";
			int width = 3;
			int height = 1;
			string anim = "utilities_conveyorbridge_kanim";
			int hitpoints = 100;
			float construction_time = 60f;
			float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
			string[] refinedMetals = MATERIALS.REFINED_METALS;
			float melting_point = 1600f;

			BuildLocationRule build_location_rule = BuildLocationRule.Conduit;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, refinedMetals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
			buildingDef.InputConduitType = ConduitType.Solid;
			buildingDef.OutputConduitType = ConduitType.Solid;
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.ViewMode = SimViewMode.SolidConveyorMap;
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 120f;
			buildingDef.ExhaustKilowattsWhenActive = 0.0f;
			buildingDef.SelfHeatKilowattsWhenActive = 2f;
			buildingDef.AudioCategory = "Metal";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.PermittedRotations = PermittedRotations.R360;
			buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
			buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
			buildingDef.PowerInputOffset = new CellOffset(0, 0);
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidConduitFilter");
			return buildingDef;
		}

		private void AttachPort(GameObject go)
		{
			go.AddComponent<ConduitSecondaryOutput>().portInfo = this.secondaryPort;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			base.DoPostConfigurePreview(def, go);
			this.AttachPort(go);
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddPrefabTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			this.AttachPort(go);

			Constructable component = go.GetComponent<Constructable>();
			component.choreTags = GameTags.ChoreTypes.ConveyorChores;
			component.requiredRolePerk = RoleManager.rolePerks.ConveyorBuild.id;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			BuildingTemplates.DoPostConfigure(go);
			go.AddOrGet<EnergyConsumer>();
			Prioritizable.AddRef(go);
			go.GetComponent<KPrefabID>().AddPrefabTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

			List<Tag> tagList = new List<Tag>();
			tagList.AddRange((IEnumerable<Tag>)STORAGEFILTERS.NOT_EDIBLE_SOLIDS);
			tagList.AddRange((IEnumerable<Tag>)STORAGEFILTERS.FOOD);

			Storage storage = go.AddOrGet<Storage>();
			storage.capacityKg = 0f;
			storage.showInUI = true;
			storage.showDescriptor = true;
			storage.storageFilters = tagList;
			storage.allowItemRemoval = false;
			storage.onlyTransferFromLowerPriority = false;

			go.AddOrGet<Automatable>();
			go.AddOrGet<TreeFilterable>();

			SolidConduitFilter filterLogic = go.AddOrGet<SolidConduitFilter>();
			filterLogic.SecondaryPort = secondaryPort;

			go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn)(game_object => new PoweredActiveController.Instance((IStateMachineTarget)game_object.GetComponent<KPrefabID>())
			{
				ShowWorkingStatus = true
			}.StartSM());
		}
	}

	[SerializationConfig(MemberSerialization.OptIn)]
	public class SolidConduitFilter : KMonoBehaviour, ISecondaryOutput
	{
		[SerializeField]
		public ConduitPortInfo SecondaryPort;

		private int inputCell = -1;
		private int outputCell = -1;
		private int filteredCell = -1;

		[MyCmpReq]
		private Storage storage;

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
