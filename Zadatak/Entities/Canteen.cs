using PetDanaUOblacima.Utils;
using System.Text.Json.Serialization;

namespace PetDanaUOblacima.Entities
{
    public class Canteen
    {
        public int Id { get; set; } 
        public string Name { get; set; } 
        public string Location { get; set; } 
        public int Capacity { get; set; } 

        public List<MealTime> WorkingHours { get; set; }
    }

    public class MealTime
    {
        public string Meal { get; set; } 
        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly From { get; set; }
        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly To { get; set; } 
    }
}
