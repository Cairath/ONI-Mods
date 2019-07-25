using System.Collections.Generic;
using Harmony;
using static CaiLib.Logger.Logger;

namespace RefinedMetalsUsableAsRawMetals
{
	public static class RefinedMetalsUsableAsRawMetalsPatches
	{
		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(ElementLoader))]
		[HarmonyPatch("FinaliseElementsTable")]
		public static class ElementLoader_LoadUserElementData_Patch
		{
			public static void Postfix()
			{
				var copper = ElementLoader.FindElementByHash(SimHashes.Copper);
				var iron = ElementLoader.FindElementByHash(SimHashes.Iron);
				var tungsten = ElementLoader.FindElementByHash(SimHashes.Tungsten);
				var gold = ElementLoader.FindElementByHash(SimHashes.Gold);
				var lead = ElementLoader.FindElementByHash(SimHashes.Lead);
				var aluminum = ElementLoader.FindElementByHash(SimHashes.Aluminum);

				var basic = new[] { copper, iron, gold, lead, aluminum };

				foreach (var element in basic)
				{
					element.oreTags = CreateTags(element.materialCategory, new[] { "Burns", "BuildableAny", "RefinedMetal", "Metal" });
					GameTags.SolidElements.Add(element.tag);
				}

				tungsten.oreTags =
					CreateTags(tungsten.materialCategory, new[] { "Plumbable", "BuildableAny", "RefinedMetal", "Metal" });
				GameTags.SolidElements.Add(tungsten.tag);
			}
		}

		private static Tag[] CreateTags(Tag materialCategory, string[] tags)
		{
			var tagList = new List<Tag>();
			if (tags != null)
			{
				foreach (var tagString in tags)
				{
					if (!string.IsNullOrEmpty(tagString))
						tagList.Add(TagManager.Create(tagString));
				}
			}

			tagList.Add(TagManager.Create(Element.State.Solid.ToString()));

			if (materialCategory.IsValid && !tagList.Contains(materialCategory))
				tagList.Add(materialCategory);

			return tagList.ToArray();
		}
	}
}
