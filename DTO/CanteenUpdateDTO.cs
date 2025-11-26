using PetDanaUOblacima.Entities;

namespace PetDanaUOblacima.DTO
{
    public class CanteenUpdateDTO
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public int? Capacity { get; set; }
        public List<MealTime>? WorkingHours { get; set; }
    }
}
