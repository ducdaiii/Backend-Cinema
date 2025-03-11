using CinemaHD.Database;
using CinemaHD.Models.Domains;
using CinemaHD.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CinemaHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet]
        [Route("get/{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest("Keyword cannot be empty.");
            }
            var users = await _context.Users
                .Where(u => u.NameUse.Contains(keyword))
                .ToListAsync();
            return Ok(users);
        }

        [Authorize]
        [HttpPut]
        [Route("update/{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UserDTO user)
        {
            // Get in4 user from token
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRoleFromToken = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdFromToken))
            {
                return Unauthorized("Invalid token.");
            }

            // Check role: Only admin or real user request can update
            if (userId != userIdFromToken && userRoleFromToken != "Admin")
            {
                return Forbid("Access denied.");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            var findRole = await _context.Roles.FirstOrDefaultAsync(r => r.NameRole == user.RoleName);

            existingUser.NameUse = user.NameUse;
            existingUser.Email = user.Email;
            existingUser.RoleID = findRole?.RoleID;
            existingUser.IsVerified = user.IsVerified;

            await _context.SaveChangesAsync();
            return Ok("User updated successfully.");
        }
    }
}
