using PetDanaUOblacima.Entities;

namespace PetDanaUOblacima.DTO
{
    public class CanteenCreateDTO
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public List<MealTime> WorkingHours { get; set; }
    }
}
