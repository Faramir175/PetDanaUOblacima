using PetDanaUOblacima.Utils;
using System.Text.Json.Serialization;

namespace PetDanaUOblacima.DTO
{
    public class ReservationCreateDTO
    {
        public int StudentId { get; set; }
        public int CanteenId { get; set; }
        public DateOnly Date { get; set; }
        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly Time { get; set; }
        public int Duration { get; set; }
    }
}
