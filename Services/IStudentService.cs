using PetDanaUOblacima.DTO;
using PetDanaUOblacima.Entities;

namespace PetDanaUOblacima.Services
{
    public interface IStudentService
    {
        Task<Student> GetStudentByIdAsync(int id);
        Task<Student> CreateStudentAsync(StudentCreateDTO dto);
        Task<Student> CheckAndGetAdminAsync(int studentId);
    }
}
