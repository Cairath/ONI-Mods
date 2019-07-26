using System.Collections.Generic;

namespace CaiLib.Utils
{
	public class RecipeUtils
	{
		public static ComplexRecipe AddComplexRecipe(ComplexRecipe.RecipeElement[] input, ComplexRecipe.RecipeElement[] output, 
			string fabricatorId, float productionTime, string recipeDescription, ComplexRecipe.RecipeNameDisplay nameDisplayType, int sortOrder, string requiredTech = null)
		{
			var recipeId = ComplexRecipeManager.MakeRecipeID(fabricatorId, input, output);

			return new ComplexRecipe(recipeId, input, output)
			{
				time = productionTime,
				description = recipeDescription,
				nameDisplay = nameDisplayType,
				fabricators = new List<Tag> { fabricatorId },
				sortOrder = sortOrder,
				requiredTech = requiredTech
			};
		}
	}
}