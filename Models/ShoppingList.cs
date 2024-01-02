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
        public Dictionary<string, bool> IngredientsToBuy { get; set; } = new Dictionary<string, bool>();
        [Ignore]
        public Dictionary<string, bool> ExtraItemstoBuy { get; set; } = new Dictionary<string, bool>();

        public string IngredientsToBuySerialized
        {
            get => JsonConvert.SerializeObject(IngredientsToBuy);
            set => IngredientsToBuy = JsonConvert.DeserializeObject<Dictionary<string, bool>>(value ?? string.Empty) ?? new Dictionary<string, bool>();
        }

        public string ExtraItemstoBuySerialized
        {
            get => JsonConvert.SerializeObject(ExtraItemstoBuy);
            set => ExtraItemstoBuy = JsonConvert.DeserializeObject<Dictionary<string, bool>>(value ?? string.Empty) ?? new Dictionary<string, bool>();
        }

    }
}
