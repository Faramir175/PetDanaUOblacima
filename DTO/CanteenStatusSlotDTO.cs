using PetDanaUOblacima.Utils;
using System.Text.Json.Serialization;

namespace PetDanaUOblacima.DTO
{
    public class CanteenStatusSlotDTO
    {
        public DateOnly Date { get; set; } 
        public string Meal { get; set; }
        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly StartTime { get; set; } 
        public int RemainingCapacity { get; set; } 
    }
}
