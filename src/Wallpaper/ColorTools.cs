using STRINGS;

namespace Wallpaper
{
	public static class ColorTools
	{
		public static void RecolorWalls()
		{
			foreach (var building in Components.BuildingCompletes.Items)
			{
				if (UI.StripLinkFormatting(building.GetProperName()) == WallpaperConfig.DisplayName)
				{
					SetColor(building);
				}
			}
		}

		public static void SetColor(BuildingComplete building)
		{
			var primaryElement = building.GetComponent<PrimaryElement>();
			var kAnimBase = building.GetComponent<KAnimControllerBase>();
			if (primaryElement == null || kAnimBase == null) return;

			var element = primaryElement.Element;
			var elementName = UI.StripLinkFormatting(element.name);

			var colorDictionary = WallpaperMod.ConfigManager.Config.Colors;
			var color = colorDictionary.ContainsKey(elementName) ? colorDictionary[elementName].ToColor() : element.substance.uiColour;

			if (element.id == SimHashes.Granite && !colorDictionary.ContainsKey(SimHashes.Granite.ToString()))
			{
				color.a = byte.MaxValue;
			}

			kAnimBase.TintColour = color;
		}
	}
}