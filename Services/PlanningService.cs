using MobileCookbook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileCookbook.Services
{
    public class PlanningService : IPlanningService
    {

        private readonly RecipeDatabase _context;

        public PlanningService(RecipeDatabase context)
        {
            _context = context;
        }

        // Insert or update MealPlanDay
        public async Task<int> UpsertMealPlanDayAsync(MealPlanDay mealPlanDay)
        {
            return await _context.UpsertMealPlanDayAsync(mealPlanDay);
        }

        // Get a single MealPlanDay by ID
        public async Task<MealPlanDay> GetMealPlanDayAsync(int id)
        {
            return await _context.GetMealPlanDayAsync(id);
        }

        // Get all MealPlanDays
        public async Task<List<MealPlanDay>> GetMealPlanDaysAsync()
        {
            return await _context.GetMealPlanDaysAsync();
        }

        public async Task<int> DeleteMealPlanDayAsync(int id)
        {
            return await _context.DeleteMealPlanDayAsync(id);
        }
    }
}
