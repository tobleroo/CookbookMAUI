using MauiCookbook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileCookbook.Models
{
    public class OneMeal
    {
        //get the id from original recipe for shopping list creation reference
        public int Id { get; set; }
        public string MealName { get; set; }
        public RecipeTypes MealType { get; set; }
        public int Portions { get; set; }
    }
}
