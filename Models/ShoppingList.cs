using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileCookbook.Models
{
    public class ShoppingList
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ListName { get; set; } = "Shopping list";

        [Ignore]
        public List<string> IngredientsToBuy { get; set; } = new List<string>();
        [Ignore]
        public List<string> ExtraItemstoBuy { get; set; } = new List<string>();

        public string IngredientsToBuySerialized
        {
            get => JsonConvert.SerializeObject(IngredientsToBuy);
            set => IngredientsToBuy = JsonConvert.DeserializeObject<List<string>>(value ?? string.Empty) ?? new List<string>();
        }

        public string ExtraItemstoBuySerialized
        {
            get => JsonConvert.SerializeObject(ExtraItemstoBuy);
            set => ExtraItemstoBuy = JsonConvert.DeserializeObject<List<string>>(value ?? string.Empty) ?? new List<string>();
        }

    }
}
