using PetDanaUOblacima.Data;
using PetDanaUOblacima.DTO;
using PetDanaUOblacima.Entities;

namespace PetDanaUOblacima.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ICanteenService _canteenService;
        private readonly IStudentService _studentService;

        public ReservationService(ICanteenService canteenService, IStudentService studentService)
        {
            _canteenService = canteenService;
            _studentService = studentService;
        }

        private bool HasOverlapWithActiveReservations(int studentId, DateOnly newDate, TimeOnly newTime, int newDuration)
        {
            var newReservationStart = newDate.ToDateTime(newTime);
            var newReservationEnd = newReservationStart.AddMinutes(newDuration);

            var studentReservations = InMemoryDbContext.GetAllReservationsForStudent(studentId)
                .Where(r => r.Status == "Active");

            foreach (var existing in studentReservations)
            {
                var existingStart = existing.Date.ToDateTime(existing.Time);
                var existingEnd = existingStart.AddMinutes(existing.Duration);

                if (newReservationStart < existingEnd && newReservationEnd > existingStart)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<Reservation> CreateReservationAsync(ReservationCreateDTO dto)
        {
             if (dto.Duration != 30 && dto.Duration != 60) {
                throw new ArgumentException("Trajanje mora biti 30 ili 60 minuta."); 
            }
             if (dto.Date < DateOnly.FromDateTime(DateTime.Today)) {
                throw new ArgumentException("Nije moguće kreirati rezervaciju za prošle dane."); 
            }
            if (dto.Time.Minute != 0 && dto.Time.Minute != 30){
                throw new ArgumentException("Vreme mora početi na pun sat ili pola sata (npr. 12:00 ili 12:30)."); 
            }

            await _studentService.GetStudentByIdAsync(dto.StudentId); 
            await _canteenService.GetCanteenByIdAsync(dto.CanteenId); 

            if (HasOverlapWithActiveReservations(dto.StudentId, dto.Date, dto.Time, dto.Duration)) {
                throw new InvalidOperationException("Student već ima aktivnu rezervaciju koja se preklapa sa traženim terminom.");
            }

            var remainingCapacity = await _canteenService.GetCanteenStatusByIdAsync(
                dto.CanteenId,
                dto.Date,
                dto.Date,
                dto.Time,
                dto.Time.AddMinutes(dto.Duration),
                dto.Duration
            );

            var slot = remainingCapacity.Slots.FirstOrDefault(s => s.Date == dto.Date && s.StartTime == dto.Time);

            if (slot == null || slot.RemainingCapacity < 1)
            {
                throw new InvalidOperationException("Menza je puna u traženom terminu."); 
            }

            var reservation = new Reservation
            {
                StudentId = dto.StudentId,
                CanteenId = dto.CanteenId,
                Date = dto.Date,
                Time = dto.Time,
                Duration = dto.Duration,
                Status = "Active"
            };

            return InMemoryDbContext.AddReservation(reservation);
        }

        public async Task<Reservation> CancelReservationAsync(int reservationId, int studentId)
        {
            var reservation = InMemoryDbContext.GetReservationById(reservationId);

            if (reservation == null)
            {
                throw new Exception("Rezervacija nije pronađena.");
            }

            if (reservation.StudentId != studentId)
            {
                throw new UnauthorizedAccessException("Student može otkazati samo sopstvene rezervacije."); 
            }

            var cancelledReservation = InMemoryDbContext.CancelReservation(reservationId);

            return cancelledReservation;
        }
    }
}
