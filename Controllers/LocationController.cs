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
    public class LocationController : ControllerBase
    {
        private readonly DataContext _context;

        public LocationController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            var locations = await _context.Locations.ToListAsync();
            return Ok(locations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocation(int id)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(l => l.LocationID == id);
            return Ok(location);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLocation([FromBody] LocationDTO locationDto)
        {
            var location = new Locations
            {
                NameLocation = locationDto.NameLocation,
                AddressBe = locationDto.AddressBe
            };
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
            return Ok("Location created successfully.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLocation([FromBody] Locations location)
        {
            var locations = await _context.Locations.FirstOrDefaultAsync(l => l.LocationID == location.LocationID);
            if (locations == null)
            {
                return NotFound("Location not found.");
            }
            locations.NameLocation = location.NameLocation;
            locations.AddressBe = location.AddressBe;
            locations.IsActive = location.IsActive;
            await _context.SaveChangesAsync();
            return Ok("Location updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(l => l.LocationID == id);

            if (location == null)
            {
                return NotFound("Location not found.");
            }

            if (location.IsActive)
            {
                return BadRequest("Cannot delete an active location.");
            }

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Location deleted successfully." });
        }
    }
}
