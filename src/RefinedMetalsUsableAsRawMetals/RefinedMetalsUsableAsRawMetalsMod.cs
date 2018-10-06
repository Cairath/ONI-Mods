using System.Collections.Generic;
using Harmony;

namespace RefinedMetalsUsableAsRawMetals
{
	public class RefinedMetalsUsableAsRawMetalsMod
	{
		[HarmonyPatch(typeof(ElementLoader), "LoadUserElementData")]
		public static class ElementLoaderLoadPatch
		{
			public static void Postfix()
			{
				Element copper = ElementLoader.FindElementByHash(SimHashes.Copper);
				Element iron = ElementLoader.FindElementByHash(SimHashes.Iron);
				Element tungsten = ElementLoader.FindElementByHash(SimHashes.Tungsten);
				Element steel = ElementLoader.FindElementByHash(SimHashes.Steel);
				Element gold = ElementLoader.FindElementByHash(SimHashes.Gold);

				var basic = new Element[] { copper, iron, gold };

				foreach (var element in basic)
				{
					element.oreTags = CreateTags(element.materialCategory, new[] { "Burns", "BuildableAny", "RefinedMetal", "Metal" });
					GameTags.SolidElements.Add(element.tag);
				}

				tungsten.oreTags =
					CreateTags(tungsten.materialCategory, new[] { "Plumbable", "BuildableAny", "RefinedMetal", "Metal" });
				GameTags.SolidElements.Add(tungsten.tag);

				steel.oreTags =
					CreateTags(steel.materialCategory, new[] { "RefinedMetal", "BuildableAny", "Metal" });
				GameTags.SolidElements.Add(steel.tag);
			}
		}

		public static Tag[] CreateTags(Tag materialCategory, string[] tags)
		{
			List<Tag> tagList = new List<Tag>();
			if (tags != null)
			{
				foreach (string tag_string in tags)
				{
					if (!string.IsNullOrEmpty(tag_string))
						tagList.Add(TagManager.Create(tag_string));
				}
			}

			tagList.Add(TagManager.Create(Element.State.Solid.ToString()));

			if (materialCategory.IsValid && !tagList.Contains(materialCategory))
				tagList.Add(materialCategory);

			return tagList.ToArray();
		}
	}
}
