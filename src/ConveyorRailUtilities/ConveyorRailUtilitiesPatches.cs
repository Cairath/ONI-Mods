using System.Collections.Generic;
using CaiLib.Utils;
using ConveyorRailUtilities.Filter;
using Harmony;
using UnityEngine;

namespace ConveyorRailUtilities
{
	public static class ConveyorRailUtilitiesPatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				CaiLib.Logger.Logger.LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				StringUtils.AddBuildingStrings(ConveyorFilterConfig.Id, ConveyorFilterConfig.DisplayName,
					ConveyorFilterConfig.Description, ConveyorFilterConfig.Effect);
				BuildingUtils.AddBuildingToPlanScreen(GameStrings.BuildingMenuCategory.Shipping, ConveyorFilterConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				BuildingUtils.AddBuildingToTechnology(GameStrings.Research.SolidMaterial.SolidTransport, ConveyorFilterConfig.Id);
			}

			[HarmonyPatch(typeof(BuildingCellVisualizer))]
			[HarmonyPatch(nameof(BuildingCellVisualizer.DrawIcons))]
			public static class BuildingCellVisualizer_DrawIcons_Patch
			{
				public static void Postfix(BuildingCellVisualizer __instance, HashedString mode)
				{
					var instance = Traverse.Create(__instance);
					var building = instance.Field("building").GetValue<Building>();

					var secondaryOutput = building.Def.BuildingComplete.GetComponent<ISecondaryOutput>();
					if (secondaryOutput == null) return;

					ConduitType secondaryConduitType = secondaryOutput.GetSecondaryConduitType();
					if (secondaryConduitType != ConduitType.Solid)
						return;

					var resources = instance.Field("resources").GetValue<BuildingCellVisualizerResources>();

					var iconsField = instance.Field("icons");
					var icons = iconsField.GetValue<Dictionary<GameObject, UnityEngine.UI.Image>>();
					var visualizerField = instance.Field("secondaryOutputVisualizer");

					if (icons == null)
					{
						return;
					}

					var visualizer = visualizerField.GetValue<GameObject>();

					if (visualizer == null)
					{
						visualizer = Util.KInstantiate(Assets.UIPrefabs.ResourceVisualizer,
							GameScreenManager.Instance.worldSpaceCanvas);
						visualizer.transform.SetAsFirstSibling();
						icons.Add(visualizer, visualizer.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>());

						visualizerField.SetValue(visualizer);

						return;
					}

					if (mode != OverlayModes.SolidConveyor.ID || !building || !resources) return;

					var offset = secondaryOutput.GetSecondaryConduitOffset();

					DrawUtilityIcon(ref icons,
						GetVisualizerCell(building, offset),
						resources.liquidOutputIcon,
						ref visualizer,
						BuildingCellVisualizer.secondOutputColour);

					visualizerField.SetValue(visualizer);
					iconsField.SetValue(icons);
				}
			}

			private static int GetVisualizerCell(Building building, CellOffset offset)
			{
				var rotatedOffset = building.GetRotatedOffset(offset);
				return Grid.OffsetCell(building.GetCell(), rotatedOffset);
			}

			private static void DrawUtilityIcon(ref Dictionary<GameObject, UnityEngine.UI.Image> icons, int cell,
				Sprite icon, ref GameObject visualizerObj, Color tint)
			{
				var posCcc = Grid.CellToPosCCC(cell, Grid.SceneLayer.Building);

				if (!visualizerObj.gameObject.activeInHierarchy)
					visualizerObj.gameObject.SetActive(true);

				visualizerObj.GetComponent<UnityEngine.UI.Image>().enabled = true;
				icons[visualizerObj].raycastTarget = false;
				icons[visualizerObj].sprite = icon;
				visualizerObj.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Image>().color = tint;
				visualizerObj.transform.SetPosition(posCcc);

				if (!(visualizerObj.GetComponent<SizePulse>() == null))
					return;

				visualizerObj.transform.localScale = Vector3.one * 1.5f;
			}
		}
	}
}