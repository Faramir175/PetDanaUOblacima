using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetDanaUOblacima.DTO;
using PetDanaUOblacima.Services;

namespace PetDanaUOblacima.Controllers
{
    [ApiController]
    [Route("canteens")]
    public class CanteenController : ControllerBase
    {
        private readonly ICanteenService _canteenService;

        public CanteenController(ICanteenService canteenService)
        {
            _canteenService = canteenService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Entities.Canteen), 201)]
        [ProducesResponseType(400)] 
        [ProducesResponseType(403)] 
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateCanteen([FromHeader(Name = "studentId")] int studentId, [FromBody] CanteenCreateDTO dto)
        {
            try
            {
                var newCanteen = await _canteenService.CreateCanteenAsync(dto, studentId);
                
                return CreatedAtAction(nameof(GetCanteenById), new { id = newCanteen.Id }, newCanteen);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(403, new { error = "Forbidden", message = "Samo studenti redari mogu da kreiraju menze." });
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

        [HttpGet]
        [ProducesResponseType(typeof(List<Entities.Canteen>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllCanteens()
        {
            try
            {
                var canteens = await _canteenService.GetAllCanteensAsync();
                return Ok(canteens);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Server error" });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Entities.Canteen), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCanteenById(int id)
        {
            try
            {
                var canteen = await _canteenService.GetCanteenByIdAsync(id);
                return Ok(canteen);
            }
            catch (Exception)
            {
                return NotFound(new { error = "Not Found", message = $"Menza with ID {id} not found." }); 
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Entities.Canteen), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateCanteen(int id, [FromHeader(Name = "studentId")] int studentId, [FromBody] CanteenUpdateDTO dto)
        {
            try
            {
                var updatedCanteen = await _canteenService.UpdateCanteenAsync(id, dto, studentId);
                return Ok(updatedCanteen);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(403, new { error = "Forbidden", message = "Samo studenti redari mogu da ažuriraju menze." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Invalid input", message = ex.Message }); 
            }
            catch (Exception)
            {
                return NotFound(new { error = "Not Found", message = $"Menza with ID {id} not found." }); 
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteCanteen(int id, [FromHeader(Name = "studentId")] int studentId)
        {
            try
            {
                await _canteenService.DeleteCanteenAsync(id, studentId);
                return NoContent(); 
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(403, new { error = "Forbidden", message = "Samo studenti redari mogu da obrišu menze." });
            }
            catch (Exception)
            {
                return NotFound(new { error = "Not Found", message = $"Menza with ID {id} not found or already deleted." }); 
            }
        }

        [HttpGet("status")]
        [ProducesResponseType(typeof(List<CanteenStatusResponseItemDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCanteensStatus(
            [FromQuery] DateOnly startDate,
            [FromQuery] DateOnly endDate,
            [FromQuery] TimeOnly startTime,
            [FromQuery] TimeOnly endTime,
            [FromQuery] int duration)
        {
            try
            {
                var status = await _canteenService.GetCanteensStatusAsync(startDate, endDate, startTime, endTime, duration);
                return Ok(status);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Invalid input", message = ex.Message }); 
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Server error" });
            }
        }

        [HttpGet("{id}/status")]
        [ProducesResponseType(typeof(CanteenStatusResponseItemDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCanteenStatusById(
            int id,
            [FromQuery] DateOnly startDate,
            [FromQuery] DateOnly endDate,
            [FromQuery] TimeOnly startTime,
            [FromQuery] TimeOnly endTime,
            [FromQuery] int duration)
        {
            try
            {
                var status = await _canteenService.GetCanteenStatusByIdAsync(id, startDate, endDate, startTime, endTime, duration);
                return Ok(status);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Invalid input", message = ex.Message }); 
            }
            catch (Exception)
            {
                return NotFound(new { error = "Not Found", message = $"Menza with ID {id} not found." }); 
            }
        }
    }
}
