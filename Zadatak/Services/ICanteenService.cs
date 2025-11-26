using PetDanaUOblacima.DTO;
using PetDanaUOblacima.Entities;

namespace PetDanaUOblacima.Services
{
    public interface ICanteenService
    {
        Task<Canteen> CreateCanteenAsync(CanteenCreateDTO dto, int studentId);
        Task<Canteen> UpdateCanteenAsync(int canteenId, CanteenUpdateDTO dto, int studentId);
        Task DeleteCanteenAsync(int canteenId, int studentId);


        Task<List<Canteen>> GetAllCanteensAsync();
        Task<Canteen> GetCanteenByIdAsync(int canteenId);


        Task<List<CanteenStatusResponseItemDTO>> GetCanteensStatusAsync(DateOnly startDate, DateOnly endDate, TimeOnly startTime, TimeOnly endTime, int duration);
        Task<CanteenStatusResponseItemDTO> GetCanteenStatusByIdAsync(int canteenId, DateOnly startDate, DateOnly endDate, TimeOnly startTime, TimeOnly endTime, int duration);
    }
}
