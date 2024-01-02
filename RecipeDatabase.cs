using MauiCookbook.Models;
using MobileCookbook.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileCookbook
{
    public class RecipeDatabase
    {
        SQLiteAsyncConnection Database;

        public RecipeDatabase()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<Recipe>();
            await Database.CreateTableAsync<Ingredient>();
            await Database.CreateTableAsync<ShoppingList>();
            await Database.CreateTableAsync<IngredientHistoryData>();
        }

        public async Task<List<Recipe>> GetRecipesAsync()
        {
            await Init();
            return await Database.Table<Recipe>().ToListAsync();
        }

        public async Task<List<IngredientHistoryData>> GetIngrHistoryAsync()
        {
            await Init();
            return await Database.Table<IngredientHistoryData>().ToListAsync();
        }

        public async Task UpsertIngredientHistoryDataAsync(IngredientHistoryData data)
        {
            await Init();

            // Check if the data already exists in the database
            var existingData = await Database.Table<IngredientHistoryData>()
                                    .Where(i => i.ID == data.ID)
                                    .FirstOrDefaultAsync();

            if (existingData != null)
            {
                // If it exists, update the existing record
                // Note: You might need to handle the TimesBought list separately 
                // if it's not properly updated by UpdateAsync
                await Database.UpdateAsync(data);
            }
            else
            {
                // If it doesn't exist, insert a new record
                await Database.InsertAsync(data);
            }
        }

        public async Task<Recipe> GetRecipeById(int id)
        {
            await Init();
            return await Database.GetAsync<Recipe>(id);
        }

        public async Task<int> SaveRecipeAsync(Recipe recipe)
        {
            await Init();
            if (recipe.ID != 0)
                return await Database.UpdateAsync(recipe);
            else
                return await Database.InsertAsync(recipe);
        }

        public async Task<int> DeleteRecipeAsync(Recipe recipe)
        {
            await Init();
            return await Database.DeleteAsync(recipe);
        }

        public async Task<int> DeleteRecipeByIdAsync(int recipeId)
        {
            await Init();

            return await Database.Table<Recipe>().DeleteAsync(r => r.ID == recipeId);
        }

        public async Task<int> SaveIngredientAsync(Ingredient ingredient)
        {
            await Init();
            if (ingredient.ID != 0)
                return await Database.UpdateAsync(ingredient);
            else
                return await Database.InsertAsync(ingredient);
        }

        public async Task<int> DeleteIngredientsByRecipeIdAsync(int recipeId)
        {
            await Init();
            return await Database.Table<Ingredient>().Where(i => i.RecipeId == recipeId).DeleteAsync();
        }

        public async Task<List<Ingredient>> GetIngredientsByRecipeIdAsync(int recipeId)
        {
            await Init();
            return await Database.Table<Ingredient>().Where(i => i.RecipeId == recipeId).ToListAsync();
        }

        public async Task<int> AddShoppingListAsync(ShoppingList shoppingList)
        {
            await Init();
            return await Database.InsertAsync(shoppingList);
        }

        public async Task<List<ShoppingList>> GetShoppingListsAsync()
        {
            await Init();
            return await Database.Table<ShoppingList>().ToListAsync();
        }

        public async Task<ShoppingList> GetShoppingListByIdAsync(int id)
        {
            await Init();
            var shoppinglist = await Database.FindAsync<ShoppingList>(id);
            return shoppinglist != null ? shoppinglist : new ShoppingList();
        }

        public async Task<ShoppingList> GetFirstOrDefaultShoppingListAsync()
        {
            await Init();
            var shoppingList = await Database.Table<ShoppingList>().FirstOrDefaultAsync();
            return shoppingList ?? new ShoppingList();
        }

        public async Task<int> UpdateShoppingListAsync(ShoppingList shoppingList)
        {
            await Init();
            return await Database.UpdateAsync(shoppingList);
        }

        public async Task<int> UpsertShoppingListAsync(ShoppingList shoppingList)
        {
            await Init();

            // Check if the shopping list already exists in the database
            var existingShoppingList = await Database.FindAsync<ShoppingList>(shoppingList.Id);

            if (existingShoppingList != null)
            {
                // Update existing shopping list
                return await Database.UpdateAsync(shoppingList);
            }
            else
            {
                // Insert new shopping list
                return await Database.InsertAsync(shoppingList);
            }
        }

        public async Task<int> DeleteShoppingListAsync(ShoppingList shoppingList)
        {
            await Init();
            return await Database.DeleteAsync(shoppingList);
        }
    }
}
