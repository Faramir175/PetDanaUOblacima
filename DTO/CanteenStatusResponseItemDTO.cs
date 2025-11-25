namespace PetDanaUOblacima.DTO
{
    public class CanteenStatusResponseItemDTO
    {
        public string CanteenId { get; set; }
        public List<CanteenStatusSlotDTO> Slots { get; set; }
    }
}
