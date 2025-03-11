using CinemaHD.Database;
using CinemaHD.Models.Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;


namespace CinemaHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly DataContext _context;

        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        public MovieController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _context.Movies.ToListAsync();
            return Ok(movies);
        }

        [HttpGet]
        [Route("get/{movieId}")]
        public async Task<IActionResult> GetMovie(string movieId)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == movieId);
            if (movie == null)
            {
                return NotFound("Movie not found.");
            }
            return Ok(movie);
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> SearchMovies([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest("Keyword cannot be empty.");
            }

            // Normalize keyword and movie fields to lowercase
            string normalizedKeyword = RemoveDiacritics(keyword.ToLower());

            var movies = await _context.Movies
                .Where(m => EF.Functions.Like(RemoveDiacritics(m.Title.ToLower()), $"%{normalizedKeyword}%") ||
                            EF.Functions.Like(RemoveDiacritics(m.Director.ToLower()), $"%{normalizedKeyword}%") ||
                            EF.Functions.Like(RemoveDiacritics(m.Details.ToLower()), $"%{normalizedKeyword}%") ||
                            EF.Functions.Like(RemoveDiacritics(m.Genre.ToLower()), $"%{normalizedKeyword}%") ||
                            EF.Functions.Like(RemoveDiacritics(m.Actors.ToLower()), $"%{normalizedKeyword}%"))
                .ToListAsync();

            if (!movies.Any())
            {
                return NotFound("No movies found matching the keyword.");
            }

            return Ok(movies);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddMovie([FromBody] Movies movie)
        {
            if (movie == null)
            {
                return BadRequest("Invalid movie data.");
            }

            // MovieID auto-generated
            movie.MovieID = UniqueCodeGenerator.GenerateUniqueCode();

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Movie added successfully.", movieId = movie.MovieID });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateMovie([FromBody] Movies movie)
        {
            if (movie == null)
            {
                return BadRequest("Invalid movie data.");
            }
            var existingMovie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == movie.MovieID);
            if (existingMovie == null)
            {
                return NotFound("Movie not found.");
            }
            existingMovie.Title = movie.Title;
            existingMovie.Director = movie.Director;
            existingMovie.Poster = movie.Poster;
            existingMovie.Details = movie.Details;
            existingMovie.Genre = movie.Genre;
            existingMovie.Duration = movie.Duration;
            existingMovie.Score = movie.Score;
            existingMovie.Actors = movie.Actors;
            existingMovie.Country = movie.Country;
            existingMovie.Subtitles = movie.Subtitles;
            existingMovie.Recommendations = movie.Recommendations;
            existingMovie.ReleaseYear = movie.ReleaseYear;
            existingMovie.Trailer = movie.Trailer;
            await _context.SaveChangesAsync();
            return Ok("Movie updated successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("delete/{movieId}")]
        public async Task<IActionResult> DeleteMovie(string movieId)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == movieId);
            if (movie == null)
            {
                return NotFound("Movie not found.");
            }

            // If `ReleaseYear` for now is 1y, acept delete
            int currentYear = DateTime.UtcNow.Year;
            int movieYear = movie.ReleaseYear.Year;

            if (currentYear - movieYear != 1)
            {
                return BadRequest("Only movies released exactly 1 year ago can be deleted.");
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return Ok("Movie deleted successfully.");
        }

    }
}
