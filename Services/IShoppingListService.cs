using MobileCookbook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileCookbook.Services
{
    public interface IShoppingListService
    {
        Task<bool> AddDateToCheckedIngredient(string ingredientName);
        Task<bool> AddRecipetoShoppingList(int recipeID, int portions);
        Task ClearTheShoppingList();
        Task<List<IngredientHistoryData>> GetIngredientHistory();
        List<string> GetRebuyRecommendations(List<IngredientHistoryData> history);
        Task<ShoppingList> GetShoppingList();
        Task<bool> UpdateShoppingListAsync(ShoppingList shoplist);
    }
}
