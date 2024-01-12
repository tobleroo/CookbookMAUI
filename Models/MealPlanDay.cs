
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileCookbook.Models
{
    public class MealPlanDay
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Ignore]
        public DateTime Date { get; set; }

        public string DateSerialized
        {
            get => JsonConvert.SerializeObject(Date);
            set => Date = string.IsNullOrEmpty(value) ? default : JsonConvert.DeserializeObject<DateTime>(value);
        }

        [Ignore]
        public List<OneMeal> Meals { get; set; } = new List<OneMeal>();

        public string MealsSerialized
        {
            get => JsonConvert.SerializeObject(Meals);
            set => Meals = JsonConvert.DeserializeObject<List<OneMeal>>(value ?? string.Empty) ?? new List<OneMeal>();
        }
    }
}
