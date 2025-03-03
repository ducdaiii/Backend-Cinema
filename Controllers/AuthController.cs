using CinemaHD.Database;
using CinemaHD.Models.Domains;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using CinemaHD.Models.DTOs;
using CinemaHD.Services;

namespace CinemaHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;

        public AuthController(DataContext context, JwtService jwtService, EmailService emailService)
        {
            _context = context;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        // Sign up
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return BadRequest("Email already exists.");

            var hashedPassword = HashPassword(request.Password);
            var idUser = UniqueCodeGenerator.GenerateUniqueCode();
            var user = new Users
            {
                UserID = idUser,
                NameUse = request.Email.Split('@')[0],
                Email = request.Email,
                Pass = hashedPassword,
                RoleID = 3,
                IsVerified = false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        // Sign in
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !VerifyPassword(request.Password, user.Pass))
                return Unauthorized("Invalid email or password.");

            var token = _jwtService.GenerateToken(user);

            return Ok(new AuthResponse
            {
                Token = token,
                UserName = user.NameUse,
                Role = user.Role?.NameRole ?? "Customer"
            });
        }

        [HttpPost("send-confirmation-email")]
        public async Task<IActionResult> SendConfirmationEmail([FromBody] EmailRequestDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return BadRequest("Người dùng không tồn tại!");

            if (user.IsVerified)
                return BadRequest("Tài khoản đã được xác nhận trước đó.");

            var token = user.UserID;

            var placeholders = new Dictionary<string, string>
            {
                { "UserName", user.NameUse },
                { "CodeRe", token }
            };

            string emailBody = await _emailService.GetEmailTemplate("Confirm-registration", placeholders);
            await _emailService.SendEmailAsync(user.Email, "Xác nhận tài khoản", emailBody);

            return Ok("Email xác nhận đã được gửi.");
        }


        [HttpPut("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string code)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == code);

            if (user == null)
                return BadRequest("Invalid confirmation.");

            user.IsVerified = true;
            await _context.SaveChangesAsync();

            return Ok("Email confirmed successfully.");
        }

        // Hash password
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        // Verify password
        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }
    }
}
