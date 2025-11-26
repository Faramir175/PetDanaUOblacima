namespace PetDanaUOblacima.DTO
{
    public class CanteenStatusResponseItemDTO
    {
        public int CanteenId { get; set; }
        public List<CanteenStatusSlotDTO> Slots { get; set; }
    }
}
