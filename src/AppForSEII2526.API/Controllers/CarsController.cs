using AppForSEII2526.API.DTOs.CarDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public CarsController(ApplicationDbContext context, ILogger<CarsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CarForPurchaseDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCars_ForPurchase(string? filtroColor, string? modelo)
        {
            var cars = await _context.Car
                .Include(c => c.Model)
                .Where(c => (c.Color.Contains(filtroColor) || filtroColor == null) && (c.Model.Name.Contains(modelo) || modelo == null))
                .Select(c => new CarForPurchaseDTO(c.Id, c.Model.Name, c.Color, c.FuelType, c.Manufacturer, c.PurchasingPrice))
                .ToListAsync();
            return Ok(cars);
        }
        
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CarForRentalDTO>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCars_ForRenting(string? modelname, float? rentingprice)
        {
            var cars = await _context.Car
                .Include(c=>c.Model)
                .Where(c => (c.Model.Name.Contains(modelname) || modelname == null )&&( c.RentingPrice < rentingprice || rentingprice == null))
                .Select(c=>new CarForRentalDTO(c.Id,c.Model.Name,c.FuelType,c.Manufacturer,c.RentingPrice,c.Color))
                .ToListAsync();
            return Ok(cars);
        }
        
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CarForReviewDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCars_ForReview(string? filtroManufacturer, string? filtroFuelType)
        {
            var cars = await _context.Car
                .Include(c => c.Model)
                .Where(c => (c.Manufacturer.Contains(filtroManufacturer) || filtroManufacturer == null) && (c.FuelType.Contains(filtroFuelType) || (filtroFuelType == null)))
                .Select(c => new CarForReviewDTO(c.Id, c.Model.Name, c.CarClass, c.Manufacturer, c.FuelType, c.Color)) 
                .ToListAsync();
            return Ok(cars);
        }
    }
}
