using MauiCookbook.Models;
using MobileCookbook;
using MobileCookbook.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCookbook.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly RecipeDatabase _context;

        public RecipeService(RecipeDatabase context)
        {
            _context = context;
        }

        public async Task<List<Recipe>> GetAllRecipesAsync()
        {
            var recipes = await _context.GetRecipesAsync();
            //fetch all ingredients for all recipes
            foreach(var recipe in recipes)
            {
                var ingredients = await _context.GetIngredientsByRecipeIdAsync(recipe.ID);
                recipe.Ingredients = ingredients;
            }

            if (recipes.Any())
            {
                return recipes;
            }
            else { return new List<Recipe>(); }
        }

        public async Task<Recipe> GetRecipeByIdAsync(int id)
        {
            var recipeWanted = await _context.GetRecipeById(id);

            //fetch ingredients belonging to recipe
            recipeWanted.Ingredients = await _context.GetIngredientsByRecipeIdAsync(id);

            return recipeWanted;
        }

        public async Task AddRecipeAsync(Recipe recipe, List<Ingredient> ingredients)
        {
            //_context.Recipes.Add(recipe);
            await _context.SaveRecipeAsync(recipe);
            //attach recipe id to all ingredients and save them to their own table
            foreach(var ingr in ingredients)
            {
                ingr.RecipeId = recipe.ID;
                await _context.SaveIngredientAsync(ingr);
            }
        }

        public async Task UpdateRecipeAsync(Recipe recipe)
        {
            
            await _context.SaveRecipeAsync(recipe);
            
            //run thu and save all ingredients
            foreach(var ingr in recipe.Ingredients)
            {
                await _context.SaveIngredientAsync(ingr);
            }
        }

        public async Task DeleteIngredient(Ingredient ingr)
        {
            await _context.DeleteIngredientAsync(ingr);
        }

        public async Task DeleteRecipeAsync(int id)
        {
            var recipe = await _context.GetRecipeById(id);
            if (recipe != null)
            {
                //delete picture if exists
                await ImageService.DeleteImage(recipe.ImageAddress);
                await _context.DeleteIngredientsByRecipeIdAsync(id);
                await _context.DeleteRecipeByIdAsync(id);
            }
        }
    }
}
