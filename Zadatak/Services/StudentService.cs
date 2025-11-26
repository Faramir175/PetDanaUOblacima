using PetDanaUOblacima.Data;
using PetDanaUOblacima.DTO;
using PetDanaUOblacima.Entities;

namespace PetDanaUOblacima.Services
{
    public class StudentService : IStudentService
    {
        public async Task<Student> GetStudentByIdAsync(int id)
        {
            var student = InMemoryDbContext.GetStudentById(id);
            if (student == null)
            {
                throw new Exception("Student not found.");
            }
            return student;
        }

        public async Task<Student> CreateStudentAsync(StudentCreateDTO dto)
        {
            var newStudent = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                IsAdmin = dto.IsAdmin
            };
            return InMemoryDbContext.AddStudent(newStudent);
        }

        public async Task<Student> CheckAndGetAdminAsync(int studentId)
        {
            var student = await GetStudentByIdAsync(studentId);

            if (!student.IsAdmin)
            {
                throw new UnauthorizedAccessException("Samo studenti redari mogu da izvrše ovu akciju.");
            }
            return student;
        }
    }
}
