using TUNING;
using UnityEngine;

namespace ConveyorDropoff
{
	public class ConveyorDropoffConfig : IBuildingConfig
	{
		public const string ID = "ConveyorDropoff";

		public override BuildingDef CreateBuildingDef()
		{
			int width = 1;
			int height = 3;
			string anim = "relocator_dropoff_kanim";
			int hitpoints = 30;
			float construction_time = 20f;
			float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
			string[] allMetals = MATERIALS.ALL_METALS;
			float melting_point = 1600f;
			BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, width, height, anim, hitpoints, construction_time, tieR3, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
			buildingDef.Floodable = false;
			buildingDef.Overheatable = false;
			buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.InputConduitType = ConduitType.Solid;
			buildingDef.UtilityInputOffset = new CellOffset(0, 1);
			buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidConduitOutbox");
			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			go.AddOrGet<ConveyorDropoff>();
			go.AddOrGet<SolidConduitConsumer>();
			Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(go);
			defaultStorage.capacityKg = 100f;
			defaultStorage.showInUI = false;
			defaultStorage.allowItemRemoval = false;
			
			go.AddOrGet<SimpleVent>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			Prioritizable.AddRef(go);
			go.AddOrGet<Automatable>();
		}
	}
}
