using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetDanaUOblacima.DTO;
using PetDanaUOblacima.Services;

namespace PetDanaUOblacima.Controllers
{
    [ApiController]
    [Route("reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Entities.Reservation), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationCreateDTO dto)
        {
            try
            {
                var newReservation = await _reservationService.CreateReservationAsync(dto);

                return StatusCode(201, newReservation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Invalid input", message = ex.Message }); 
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Invalid input", message = ex.Message }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Server error", message = ex.Message }); 
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Entities.Reservation), 200)]
        [ProducesResponseType(403)] 
        [ProducesResponseType(404)] 
        [ProducesResponseType(500)]
        public async Task<IActionResult> CancelReservation(int id, [FromHeader(Name = "studentId")] int studentId)
        {
            try
            {
                var cancelledReservation = await _reservationService.CancelReservationAsync(id, studentId);
                return Ok(cancelledReservation); 
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { error = "Forbidden", message = ex.Message });
            }
            catch (Exception)
            {
                return NotFound(new { error = "Not Found", message = $"Reservation with ID {id} not found." }); 
            }
        }
    }
}
