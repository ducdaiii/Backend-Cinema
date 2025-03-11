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
    public class ShowTController : ControllerBase
    {
        private readonly DataContext _context;

        public ShowTController(DataContext dataContext)
        {
            _context = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var show = await _context.MovieShowtimes.ToListAsync();
            return Ok(show); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var show = await _context.MovieShowtimes.FirstOrDefaultAsync(x => x.CinemaID == id);
            return Ok(show);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieShowtimeDTO cinemaDTO)
        {
            var movieIdResult = await _context.Movies
                .Where(c => c.Title == cinemaDTO.CinemaName)
                .Select(c => c.MovieID)
                .FirstOrDefaultAsync();

            var cinemaIdResult = await _context.Cinemas
                .Where(c => c.NameCinema == cinemaDTO.CinemaName)
                .Select(c => c.CinemaID)
                .FirstOrDefaultAsync();

            if (movieIdResult == null)
            {
                return BadRequest("Movie not found.");
            }

            if (movieIdResult == null)
            {
                return BadRequest("Movie not found.");
            }

            var show = new MovieShowtimes
            {
                MovieID = movieIdResult,
                CinemaID = cinemaIdResult,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
            };
            _context.MovieShowtimes.Add(show);
            await _context.SaveChangesAsync();
            return Ok(show);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MovieShowtimeDTO cinemaDTO)
        {
            var show = await _context.MovieShowtimes.FirstOrDefaultAsync(x => x.ShowtimeID == id);
            if (show == null)
            {
                return NotFound();
            }
            var movieIdResult = await _context.Movies
                .Where(c => c.Title == cinemaDTO.CinemaName)
                .Select(c => c.MovieID)
                .FirstOrDefaultAsync();
            var cinemaIdResult = await _context.Cinemas
                .Where(c => c.NameCinema == cinemaDTO.CinemaName)
                .Select(c => c.CinemaID)
                .FirstOrDefaultAsync();
            if (movieIdResult == null)
            {
                return BadRequest("Movie not found.");
            }
            if (movieIdResult == null)
            {
                return BadRequest("Movie not found.");
            }
            show.MovieID = movieIdResult;
            show.CinemaID = cinemaIdResult;
            show.StartTime = DateTime.Now;
            show.EndTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(show);
        }
    }
}
