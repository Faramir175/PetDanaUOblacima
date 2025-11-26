using PetDanaUOblacima.Entities;

namespace PetDanaUOblacima.Data
{
    public class InMemoryDbContext
    {
        private static List<Student> _students = new List<Student>();
        private static List<Canteen> _canteens = new List<Canteen>();
        private static List<Reservation> _reservations = new List<Reservation>();

        // Brojaci za simulaciju auto increment ID-jeva
        private static int _studentIdCounter = 1;
        private static int _canteenIdCounter = 1;
        private static int _reservationIdCounter = 1;


        public static Student AddStudent(Student student)
        {
            if (_students.Any(s => s.Email.Equals(student.Email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Student sa tim emailom vec postoji.");
            }

            student.Id = _studentIdCounter++;
            _students.Add(student);
            return student;
        }

        public static Student GetStudentById(int id)
        {
            return _students.FirstOrDefault(s => s.Id == id);
        }

        public static Student GetStudentByEmail(string email)
        {
            return _students.FirstOrDefault(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public static Canteen AddCanteen(Canteen canteen)
        {
            if (_canteens.Any(c => c.Name.Equals(canteen.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Menza sa tim imenom vec postoji.");
            }

            canteen.Id = _canteenIdCounter++;
            _canteens.Add(canteen);
            return canteen;
        }

        public static List<Canteen> GetAllCanteens()
        {
            return _canteens;
        }

        public static Canteen GetCanteenById(int id)
        {
            return _canteens.FirstOrDefault(c => c.Id == id);
        }

        public static Canteen UpdateCanteen(Canteen updatedCanteen)
        {
            var existingCanteen = _canteens.FirstOrDefault(c => c.Id == updatedCanteen.Id);
            if (existingCanteen == null)
            {
                return null;
            }

            existingCanteen.Name = updatedCanteen.Name;
            existingCanteen.Location = updatedCanteen.Location;
            existingCanteen.Capacity = updatedCanteen.Capacity;
            existingCanteen.WorkingHours = updatedCanteen.WorkingHours;

            return existingCanteen;
        }

        public static bool DeleteCanteen(int id)
        {
            var canteenToRemove = _canteens.FirstOrDefault(c => c.Id == id);
            if (canteenToRemove == null)
            {
                return false; 
            }

            var reservationsToCancel = _reservations.Where(r => r.CanteenId == id && r.Status == "Active").ToList();

            foreach (var reservation in reservationsToCancel)
            {
                reservation.Status = "Cancelled";
            }

            return _canteens.Remove(canteenToRemove);
        }

        public static Reservation AddReservation(Reservation reservation)
        {
            reservation.Id = _reservationIdCounter++;
            _reservations.Add(reservation);
            return reservation;
        }

        public static Reservation GetReservationById(int id)
        {
            return _reservations.FirstOrDefault(r => r.Id == id);
        }

        public static List<Reservation> GetActiveReservations()
        {
            return _reservations.Where(r => r.Status == "Active").ToList();
        }

        public static List<Reservation> GetAllReservationsForStudent(int studentId)
        {
            return _reservations.Where(r => r.StudentId == studentId).ToList();
        }

        public static Reservation CancelReservation(int id)
        {
            var reservationToCancel = _reservations.FirstOrDefault(r => r.Id == id);
            if (reservationToCancel == null)
            {
                return null;
            }

            if (reservationToCancel.Status != "Cancelled")
            {
                reservationToCancel.Status = "Cancelled"; 
            }

            return reservationToCancel;
        }

        public static void ResetDatabase()
        {
            _students.Clear();
            _canteens.Clear();
            _reservations.Clear();

            _studentIdCounter = 1;
            _canteenIdCounter = 1;
            _reservationIdCounter = 1;
        }
    }
}