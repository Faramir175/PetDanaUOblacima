namespace PetDanaUOblacima.DTO
{
    public class ReservationCreateDTO
    {
        public int StudentId { get; set; }
        public int CanteenId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public int Duration { get; set; }
    }
}
