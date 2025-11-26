using PetDanaUOblacima.Data;
using PetDanaUOblacima.DTO;
using PetDanaUOblacima.Entities;

namespace PetDanaUOblacima.Services
{
    public class CanteenService : ICanteenService
    {
        private readonly IStudentService _studentService;

        public CanteenService(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<List<Canteen>> GetAllCanteensAsync()
        {
            return InMemoryDbContext.GetAllCanteens();
        }

        public async Task<Canteen> GetCanteenByIdAsync(int canteenId)
        {
            var canteen = InMemoryDbContext.GetCanteenById(canteenId);
            if (canteen == null)
            {
                throw new Exception("Menza nije pronađena."); 
            }
            return canteen;
        }

        public async Task<Canteen> CreateCanteenAsync(CanteenCreateDTO dto, int studentId)
        {
            await _studentService.CheckAndGetAdminAsync(studentId);

            var canteen = new Canteen
            {
                Name = dto.Name,
                Location = dto.Location,
                Capacity = dto.Capacity,
                WorkingHours = dto.WorkingHours
            };

            return InMemoryDbContext.AddCanteen(canteen);
        }

        public async Task<Canteen> UpdateCanteenAsync(int canteenId, CanteenUpdateDTO dto, int studentId)
        {
            await _studentService.CheckAndGetAdminAsync(studentId);

            var existingCanteen = InMemoryDbContext.GetCanteenById(canteenId);
            if (existingCanteen == null)
            {
                throw new Exception("Menza nije pronađena za ažuriranje."); 
            }

            if (!string.IsNullOrEmpty(dto.Name))
            {
                existingCanteen.Name = dto.Name;
            }
            if (!string.IsNullOrEmpty(dto.Location))
            {
                existingCanteen.Location = dto.Location;
            }

            if (dto.Capacity.HasValue && dto.Capacity.Value >= 0)
            {
                existingCanteen.Capacity = dto.Capacity.Value;
            }

            if (dto.WorkingHours != null)
            {
                existingCanteen.WorkingHours = dto.WorkingHours;
            }

            return InMemoryDbContext.UpdateCanteen(existingCanteen);
        }

        public async Task DeleteCanteenAsync(int canteenId, int studentId)
        {
            await _studentService.CheckAndGetAdminAsync(studentId);

            var success = InMemoryDbContext.DeleteCanteen(canteenId);

            if (!success)
            {
                throw new Exception("Menza nije pronađena za brisanje.");
            }
        }

        private List<TimeOnly> GenerateIntervals(TimeOnly startTime, TimeOnly endTime)
        {
            var slots = new List<TimeOnly>();
            var currentTime = startTime;

            while (currentTime < endTime)
            {
                if (currentTime.Minute == 0 || currentTime.Minute == 30)
                {
                    slots.Add(currentTime);
                }
                currentTime = currentTime.AddMinutes(30);
            }
            return slots;
        }

        private int CalculateRemainingCapacity(Canteen canteen, DateOnly date, TimeOnly slotTime, int duration)
        {
            var slotEnd = slotTime.AddMinutes(duration);

            var mealTime = canteen.WorkingHours.FirstOrDefault(wh =>
                wh.From <= slotTime &&
                wh.To >= slotEnd
            );

            if (mealTime == null)
            {
                return 0;
            }

            var activeReservations = InMemoryDbContext.GetActiveReservations()
                .Where(r => r.CanteenId == canteen.Id && r.Date == date)
                .ToList();

            int occupiedSeats = 0;

            foreach (var reservation in activeReservations)
            {
                var reservationStart = reservation.Time;
                var reservationEnd = reservationStart.AddMinutes(reservation.Duration);

                if (slotTime < reservationEnd && slotEnd > reservationStart)
                {
                    occupiedSeats++;
                }
            }
            return canteen.Capacity - occupiedSeats;
        }
        public async Task<CanteenStatusResponseItemDTO> GetCanteenStatusByIdAsync(int canteenId, DateOnly startDate, DateOnly endDate, TimeOnly startTime, TimeOnly endTime, int duration)
        {
            if (duration != 30 && duration != 60)
            {
                throw new ArgumentException("Trajanje obroka mora biti 30 ili 60 minuta.");
            }

            var canteen = InMemoryDbContext.GetCanteenById(canteenId);
            if (canteen == null)
            {
                throw new Exception("Menza nije pronađena."); 
            }

            var result = new CanteenStatusResponseItemDTO
            {
                CanteenId = canteenId,
                Slots = new List<CanteenStatusSlotDTO>()
            };

            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                var intervals = GenerateIntervals(startTime, endTime);

                foreach (var slotTime in intervals)
                {
                    if (duration == 60 && slotTime.Minute == 30)
                    {
                        continue;
                    }

                    var slotEnd = slotTime.AddMinutes(duration);

                    var mealTime = canteen.WorkingHours.FirstOrDefault(wh =>
                        wh.From <= slotTime &&
                        wh.To >= slotEnd
                    );

                    if (mealTime != null)
                    {
                        var remaining = CalculateRemainingCapacity(canteen, currentDate, slotTime, duration);

                        if (remaining > 0)
                        {
                            result.Slots.Add(new CanteenStatusSlotDTO
                            {
                                Date = currentDate,
                                Meal = mealTime.Meal,
                                StartTime = slotTime,
                                RemainingCapacity = remaining
                            });
                        }
                    }
                }
                currentDate = currentDate.AddDays(1);
            }

            return result;
        }

        public async Task<List<CanteenStatusResponseItemDTO>> GetCanteensStatusAsync(DateOnly startDate, DateOnly endDate, TimeOnly startTime, TimeOnly endTime, int duration)
        {
            if (duration != 30 && duration != 60)
            {
                 throw new ArgumentException("Trajanje obroka mora biti 30 ili 60 minuta.");
            }

            var allCanteens = InMemoryDbContext.GetAllCanteens();
            var allStatuses = new List<CanteenStatusResponseItemDTO>();

            foreach (var canteen in allCanteens)
            {
                var status = await GetCanteenStatusByIdAsync(canteen.Id, startDate, endDate, startTime, endTime, duration);

                if (status.Slots.Any())
                {
                    allStatuses.Add(status);
                }
            }
            return allStatuses;
        }
    }
}
