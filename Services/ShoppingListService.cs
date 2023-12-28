using MobileCookbook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileCookbook.Services
{
    public class ShoppingListService : IShoppingListService
    {

        private readonly RecipeDatabase _recipeDb;

        public ShoppingListService(RecipeDatabase recipeDb)
        {
            _recipeDb = recipeDb;
        }

        public async Task<ShoppingList> GetShoppingList()
        {
            return await _recipeDb.GetFirstOrDefaultShoppingListAsync();
        }

        public async Task<bool> UpdateShoppingListAsync(ShoppingList shoplist)
        {
            var success = await _recipeDb.UpdateShoppingListAsync(shoplist);
            return success > 0;
        }

        public async Task<bool> AddRecipetoShoppingList(int recipeID, int portions)
        {
            try
            {
                var recipeWanted = await _recipeDb.GetRecipeById(recipeID);

                // If the recipe is not found, return false
                if (recipeWanted == null)
                    return false;

                // Get the shopping list 
                var shoppinglist = await _recipeDb.GetFirstOrDefaultShoppingListAsync();

                // Run through all ingredients from recipe and add their name to shopping list items
                foreach (var ingredient in recipeWanted.Ingredients)
                {
                    // Add portions to the name string
                    shoppinglist.IngredientsToBuy.Add($"{ingredient.Name} {(ingredient.Quantity / recipeWanted.Portions) * portions} {ingredient.Unit}");
                }

                // Then update the shopping list
                await _recipeDb.UpsertShoppingListAsync(shoppinglist);

                return true; // Operation successful
            }
            catch
            {
                return false; // Operation failed
            }
        }


    }
}
