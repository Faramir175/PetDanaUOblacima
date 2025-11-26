using PetDanaUOblacima.DTO;
using PetDanaUOblacima.Entities;

namespace PetDanaUOblacima.Services
{
    public interface IReservationService
    {
        Task<Reservation> CreateReservationAsync(ReservationCreateDTO dto);

        Task<Reservation> CancelReservationAsync(int reservationId, int studentId);
    }
}
