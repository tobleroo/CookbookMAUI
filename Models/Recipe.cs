using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCookbook.Models
{
    public class Recipe
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string RecipeName { get; set; }
        public string? Description { get; set; } = "not set";

        //public string? ImageAddress { get; set; } = "resource://MobileCookbook.Resources.Images.noimage.jpg";
        public string StepsSerialized
        {
            get => JsonConvert.SerializeObject(Steps);
            set => Steps = JsonConvert.DeserializeObject<List<string>>(value);
        }

        [Ignore]
        public List<string> Steps { get; set; } = new();

        [Ignore]
        public List<Ingredient> Ingredients { get; set; } = new();

        public int CookTime { get; set; } = 0;

        public RecipeDifficulties Difficulty { get; set; } = RecipeDifficulties.Easy;
        public int TimesCooked { get; set; } = 0;
        public bool IsVegan { get; set; } = false;

        public int Portions { get; set; } = 1;

        public RecipeTypes RecipeType { get; set; } = RecipeTypes.Breakfast;

        private int _spicyness = 1;
        public int Spicyness
        {
            get { return _spicyness; }
            set
            {
                if (value >= 1 && value <= 5) _spicyness = value;
                else throw new ArgumentOutOfRangeException("Spicyness must be between 1 and 5.");
            }
        }
    }
}
