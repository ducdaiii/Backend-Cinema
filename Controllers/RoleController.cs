using CinemaHD.Database;
using CinemaHD.Models.Domains;
using CinemaHD.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly DataContext _context;

        public RoleController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpGet]
        [Route("get/{roleId}")]
        public async Task<IActionResult> GetRole(int roleId)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleID == roleId);
            if (role == null)
            {
                return NotFound("Role not found.");
            }
            return Ok(role);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateRole([FromBody] RoleDTO roleDTO)
        {
            var role = new Roles
            {
                NameRole = roleDTO.NameRole
            };
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return Ok("Role created successfully.");
        }
    }
}
