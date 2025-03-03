using CinemaHD.Database;
using CinemaHD.Models.DTOs;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;
        private readonly DataContext _context;

        public EmailController(EmailService emailService, DataContext dataContext)
        {
            _emailService = emailService;
            _context = dataContext;
        }

        [HttpPost("send-notification-email")]
        public async Task<IActionResult> SendNotificationEmail([FromBody] MovieNotificationDTO movieDTO)
        {
            var users = await _context.Users.ToListAsync();
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == movieDTO.MovieId);

            if (users == null || users.Count == 0)
                return BadRequest("Database null!");

            foreach (var user in users)
            {
                if (user.IsVerified)
                {
                    var placeholders = new Dictionary<string, string>
                    {
                        { "User", user.NameUse },
                        { "MoviePosterUrl", movie.Poster },
                        { "MovieName", movie.Title },
                        { "Trailer", movie.Trailer }
                    };

                    string emailBody = await _emailService.GetEmailTemplate("General-notification", placeholders);
                    await _emailService.SendEmailAsync(user.Email, "Thư mới gửi bạn 🍿🎥", emailBody);
                }
            }

            return Ok("Email send to all user verified.");
        }
    }
}
