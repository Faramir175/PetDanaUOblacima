namespace PetDanaUOblacima.DTO
{
    public class CanteenStatusSlotDTO
    {
        public DateOnly Date { get; set; } 
        public string Meal { get; set; } 
        public TimeOnly StartTime { get; set; } 
        public int RemainingCapacity { get; set; } 
    }
}
