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
    public class SeatController : ControllerBase
    {
        private readonly DataContext _context;

        public SeatController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSeats()
        {
            var seats = await _context.Seats.ToListAsync();
            return Ok(seats);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeat(int id)
        {
            var seat = await _context.Seats.FirstOrDefaultAsync(s => s.SeatID == id);
            return Ok(seat);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSeat([FromBody] SeatDTO seatDTO)
        {
            var seat = new Seats
            {
                AtRow = new string(seatDTO.SeatPosition.Where(char.IsLetter).ToArray()),
                AtColumn = seatDTO.SeatPosition.First(char.IsDigit),
                IsActive = seatDTO.IsActive,
                CinemaID = await _context.Cinemas
                    .Where(c => c.NameCinema == seatDTO.CinemaName)
                    .Select(c => c.CinemaID)
                    .FirstOrDefaultAsync()
            };
            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();
            return Ok("Seat created successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeat(int id, [FromBody] SeatDTO seatDTO)
        {
            var seat = await _context.Seats.FirstOrDefaultAsync(s => s.SeatID == id);
            if (seat == null)
            {
                return NotFound();
            }
            seat.AtRow = new string(seatDTO.SeatPosition.Where(char.IsLetter).ToArray());
            seat.AtColumn = seatDTO.SeatPosition.First(char.IsDigit);
            seat.IsActive = seatDTO.IsActive;
            seat.CinemaID = await _context.Cinemas
                .Where(c => c.NameCinema == seatDTO.CinemaName)
                .Select(c => c.CinemaID)
                .FirstOrDefaultAsync();
            await _context.SaveChangesAsync();
            return Ok("Seat updated successfully.");
        }
    }
}
