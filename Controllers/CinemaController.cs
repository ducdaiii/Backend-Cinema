using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaHD.Database;
using CinemaHD.Models.Domains;
using CinemaHD.Models.DTOs;

namespace CinemaHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemaController : ControllerBase
    {
        private readonly DataContext _context;

        public CinemaController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCinemas()
        {
            var cinemas = await _context.Cinemas.ToListAsync();
            return Ok(cinemas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCinema(int id)
        {
            var cinema = await _context.Cinemas.FirstOrDefaultAsync(c => c.CinemaID == id);
            return Ok(cinema);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCinema([FromBody] CinemaDTO cinemaDto)
        {
            var cinema = new Cinemas
            {
                NameCinema = cinemaDto.NameCinema,
                IsActive = cinemaDto.IsActive,
                LocationID = await _context.Locations
                    .Where(l => l.NameLocation == cinemaDto.LocationName)
                    .Select(l => l.LocationID)
                    .FirstOrDefaultAsync()
            };
            _context.Cinemas.Add(cinema);
            await _context.SaveChangesAsync();
            return Ok("Cinema created successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCinema(int id, [FromBody] CinemaDTO cinemaDto)
        {
            var cinema = await _context.Cinemas.FirstOrDefaultAsync(c => c.CinemaID == id);
            if (cinema == null)
            {
                return NotFound();
            }
            cinema.NameCinema = cinemaDto.NameCinema;
            cinema.IsActive = cinemaDto.IsActive;
            cinema.LocationID = await _context.Locations
                .Where(l => l.NameLocation == cinemaDto.LocationName)
                .Select(l => l.LocationID)
                .FirstOrDefaultAsync();
            await _context.SaveChangesAsync();
            return Ok("Cinema updated successfully.");
        }
    }
}
