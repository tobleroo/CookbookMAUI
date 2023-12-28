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
        Task<bool> AddRecipetoShoppingList(int recipeID, int portions);
        Task<ShoppingList> GetShoppingList();
        Task<bool> UpdateShoppingListAsync(ShoppingList shoplist);
    }
}
