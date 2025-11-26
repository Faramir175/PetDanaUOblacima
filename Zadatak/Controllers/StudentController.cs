using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetDanaUOblacima.DTO;
using PetDanaUOblacima.Services;

namespace PetDanaUOblacima.Controllers
{
    [ApiController]
    [Route("students")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Entities.Student), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateStudent([FromBody] StudentCreateDTO dto)
        {
            try
            {
                var newStudent = await _studentService.CreateStudentAsync(dto);
                
                return CreatedAtAction(nameof(GetStudentById), new { id = newStudent.Id }, newStudent);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Invalid input", message = ex.Message }); 
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Server error" });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Entities.Student), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetStudentById(int id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                
                return Ok(student);
            }
            catch (Exception)
            {
                return NotFound(new { error = "Not Found", message = $"Student with ID {id} not found." });
            }
        }
    }
}
