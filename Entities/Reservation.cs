namespace PetDanaUOblacima.Entities
{
    public class Reservation
    {
        public int Id { get; set; } 
        public int StudentId { get; set; } 
        public int CanteenId { get; set; } 

        public DateOnly Date { get; set; }

        public TimeOnly Time { get; set; }

        public int Duration { get; set; }

        public string Status { get; set; } = "Active";
    }
}
