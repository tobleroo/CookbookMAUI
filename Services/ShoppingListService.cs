using MauiCookbook.Models;
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

        public async Task ClearTheShoppingList()
        {
            await _recipeDb.ClearShoppingListAsync();
        }

        public async Task ClearRecipeIngredientsFromShoppingList()
        {
            var shoppinglist =  await _recipeDb.GetFirstOrDefaultShoppingListAsync();
            if (shoppinglist != null)
            {
                shoppinglist.IngredientsToBuy.Clear();
            }
            await _recipeDb.UpsertShoppingListAsync(shoppinglist);
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

                // Get ingredients
                var ingredientsForRecipe = await _recipeDb.GetIngredientsByRecipeIdAsync(recipeID);
                //recipeWanted.Ingredients = ingredientsForRecipe;

                // If the recipe is not found, return false
                if (ingredientsForRecipe == null || ingredientsForRecipe.Count == 0)
                    return false;

                // Get the shopping list 
                var shoppinglist = await _recipeDb.GetFirstOrDefaultShoppingListAsync();

                // since i cant alter/change in dictionary, create a new dictionary to return
                Dictionary<string, bool> updatedIngredientsShoppingList = new();

                //run thru the recipe inngredient list of the shoppinglist
                // to try and find if ingredient name already exists 
                foreach(KeyValuePair<string,bool> entry in shoppinglist.IngredientsToBuy)
                {
                    //split the name on the : to only get the name of the ingredient without any added amounts
                    string pureName = entry.Key.Split(":")[0];
                    for (int i = ingredientsForRecipe.Count - 1; i >= 0; i--)
                    {
                        if(pureName == ingredientsForRecipe[i].Name)
                        {
                            //if yes -> append the amount from the new duplicate to the existing name
                            double? RecalcIngrAmount = (ingredientsForRecipe[i].Quantity / recipeWanted.Portions) * portions;
                            string newAddedPortionsAmount = $" + {RecalcIngrAmount} {ingredientsForRecipe[i].Unit}";
                            //since keys name are immutable, i have to remove and add a new updated name pair

                            // Create the new key name
                            string updatedKeyName = entry.Key;
                            updatedKeyName = string.Concat(updatedKeyName, newAddedPortionsAmount);

                            // Add the new key-value pair
                            updatedIngredientsShoppingList.Add(updatedKeyName, entry.Value);

                            // remove ingredient from the recipe ingredient list 
                            // so i can add the rest that didnt have duplicates
                            ingredientsForRecipe.Remove(ingredientsForRecipe[i]);
                            break;
                        }
                    }
                }

                //if no -> do the string manipulating and add as new item
                foreach (var ingredient in ingredientsForRecipe)
                {
                    // add the portions re-calculated amount to the name string to be seen in the shoppinglist
                    string itemString = $"{ingredient.Name}: {(ingredient.Quantity / recipeWanted.Portions) * portions} {ingredient.Unit}";

                    updatedIngredientsShoppingList.Add(itemString, false);
                    // Add or update the item in the dictionary with a default unchecked state (false)
                    //shoppinglist.IngredientsToBuy[itemString] = false;
                }

                shoppinglist.IngredientsToBuy = updatedIngredientsShoppingList;
                // Then update the shopping list
                await _recipeDb.UpsertShoppingListAsync(shoppinglist);

                return true; // Operation successful
            }
            catch(Exception ex)
            {
                Console.WriteLine("error -> : "+ ex.Message);
                return false; // Operation failed
            }
        }

        public async Task SaveNewShopinglist(ShoppingList shoppingList)
        {
            await _recipeDb.UpsertShoppingListAsync(shoppingList);
        }

        public async Task<List<IngredientHistoryData>> GetIngredientHistory()
        {
            try
            {
                return await _recipeDb.GetIngrHistoryAsync();
            }catch (Exception ex) { return new List<IngredientHistoryData>();}
        }

        public List<string> GetRebuyRecommendations(List<IngredientHistoryData> history)
        {
            var today = DateTime.Today;
            var recommendations = new List<string>();

            foreach (var ingredient in history)
            {
                if (ingredient.TimesBought.Count < 2)
                {
                    // Need at least two dates to calculate an average
                    continue;
                }

                // Calculate average interval
                var intervals = new List<TimeSpan>();
                for (int i = 1; i < ingredient.TimesBought.Count; i++)
                {
                    intervals.Add(ingredient.TimesBought[i] - ingredient.TimesBought[i - 1]);
                }
                var averageInterval = TimeSpan.FromTicks((long)intervals.Average(interval => interval.Ticks));

                // Check if the current interval since the last purchase exceeds the average
                var lastPurchase = ingredient.TimesBought.Last();
                if (today - lastPurchase > averageInterval)
                {
                    recommendations.Add(ingredient.Name);
                }
            }

            return recommendations;
        }

        public async Task<bool> AddDateToCheckedIngredient(string ingredientName)
        {
            try
            {

                //get the list from db
                var ingrHistory = await _recipeDb.GetIngrHistoryAsync();

                //check if name exists or create new object
                var existingObj = ingrHistory.Find(item => item.Name == ingredientName);
                existingObj ??= new IngredientHistoryData() { Name = ingredientName };

                // Add today's date to the TimesBought list
                existingObj.TimesBought.Add(DateTime.Today);
            
                //upsert the ingredientHistory object to the table
                await _recipeDb.UpsertIngredientHistoryDataAsync(existingObj);
                return true;
            }
            catch { return false; }
        }


    }
}
