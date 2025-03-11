using CinemaHD.Database;
using CinemaHD.Models.Domains;
using CinemaHD.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly DataContext _context;

        public ReservationController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetReservations()
        {
            var reservations = await _context.Reservations.ToListAsync();
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservation(int id)
        {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.ReservationID == id);
            return Ok(reservation);
        }

        [HttpGet]
        [Route("from-to")]
        public async Task<IActionResult> GetReservationsFromTo([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var reservations = await _context.Reservations
                .Where(r => r.TimeReservation >= from && r.TimeReservation <= to)
                .ToListAsync();
            return Ok(reservations);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationDTO reservationDTO)
        {
            var userId = HttpContext.Items["UserId"]?.ToString();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User authentication required.");
            }

            var seat = await _context.Seats.FirstOrDefaultAsync(s => s.Position == reservationDTO.SeatPosition);
            if (seat == null)
            {
                return BadRequest("Seat not found.");
            }

            var reservation = new Reservations
            {
                TimeReservation = reservationDTO.TimeReservation,
                UserID = userId,
                SeatID = seat.SeatID
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return Ok("Reservation created successfully.");
        }
    }
}
