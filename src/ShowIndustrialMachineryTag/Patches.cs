using Harmony;
using UnityEngine;

namespace ShowIndustrialMachineryTag
{
	public static class Patches
	{
		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				CaiLib.ModCounter.ModCounter.Hit(ModInfo.Name, ModInfo.Version);
				CaiLib.Logger.LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(ProductInfoScreen))]
		[HarmonyPatch("SetDescription")]
		public static class ProductInfoScreen_SetDescription_Patch
		{
			public static void Postfix(ref ProductInfoScreen __instance, BuildingDef def)
			{
				__instance.productFlavourText.text += "\n\n<b>This is a test</b>";
			}
		}

		[HarmonyPatch(typeof(SimpleInfoScreen))]
		[HarmonyPatch("SetPanels")]
		public static class SimpleInfoScreen_SetPanels_Patch
		{
			public static void Postfix(ref SimpleInfoScreen __instance, GameObject target)
			{
				if (target.GetComponent<KPrefabID>().HasTag(RoomConstraints.ConstraintTags.IndustrialMachinery))
				{
					var field = Traverse.Create(__instance).Field("descriptionContainer");
					var descriptionContainer = field.GetValue<DescriptionContainer>();
					descriptionContainer.flavour.text += "\n\n<color=\"red\"><b>This is an industrial machine.</b></color>";

					field.SetValue(descriptionContainer);
				}
			}
		}

	}
}
