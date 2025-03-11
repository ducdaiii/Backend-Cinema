using CinemaHD.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ShiftController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


    }
}
