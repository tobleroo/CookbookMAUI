using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileCookbook.Models
{
    public class IngredientHistoryData
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }

        [Ignore]
        public List<DateTime> TimesBought { get; set; } = new List<DateTime>();

        public string TimesBoughtSerialized
        {
            get => JsonConvert.SerializeObject(TimesBought);
            set => TimesBought = JsonConvert.DeserializeObject<List<DateTime>>(value ?? string.Empty) ?? new List<DateTime>();
        }
    }
}
