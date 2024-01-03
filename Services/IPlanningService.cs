using MobileCookbook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileCookbook.Services
{
    internal interface IPlanningService
    {
        Task<int> DeleteMealPlanDayAsync(int id);
        Task<MealPlanDay> GetMealPlanDayAsync(int id);
        Task<List<MealPlanDay>> GetMealPlanDaysAsync();
        Task<int> UpsertMealPlanDayAsync(MealPlanDay mealPlanDay);
    }
}
