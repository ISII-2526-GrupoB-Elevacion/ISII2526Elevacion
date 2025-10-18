using AppForSEII2526.API.DTOs.CarDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        //used to enable your controller to access to the database
        private readonly ApplicationDbContext _context;
        //used to log any information when your system is running
        private readonly ILogger<CarsController> _logger;

        public CarsController(ApplicationDbContext _context, ILogger<CarsController> _logger)
        {
            this._context = _context;
            this._logger = _logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CarForRentalDTO>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCars_ForRenting_DTO(string? modelname, float? rentingprice)
        {
            IList<CarForRentalDTO> cars = await _context.Car
                .Include(c=>c.Model)
                .Where(c => (c.Model.Name.Contains(modelname) || modelname == null )&&( c.RentingPrice < rentingprice || rentingprice == null))
                .Select(c=>new CarForRentalDTO(c.Id,c.Model.Name,c.FuelType,c.Manufacturer,c.RentingPrice,c.Color))
                .ToListAsync();
            return Ok(cars);
        }
    }
}
