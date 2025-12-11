using AppForSEII2526.API.DTOs.CarDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;


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
            _logger.LogInformation("Controller 'CarsController' initialized");
        }

        /*si no se le pasan parámetros imprime todos los coches, si se la pasa uno imprime aquellos
        que cumplan con el requisito, igual que cuando se le pasan 2 parámetros*/

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CarForPurchaseDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCarsForPurchase(string? filtroColor, string? modelo)
        {
            var cars = await _context.Car
                .Include(c => c.Model)
                .Where(c => (c.Color.Contains(filtroColor) || filtroColor == null) && (c.Model.Name.Contains(modelo) || modelo == null) && c.QuantityForPurchasing>0)
                .Select(c => new CarForPurchaseDTO(c.Id, c.Model.Name, c.Color, c.Description, c.FuelType, c.Manufacturer, c.PurchasingPrice))
                .ToListAsync();
            _logger.LogInformation($"CarsController || Cars for purchase found with the parameters {filtroColor} and {modelo}");
            return Ok(cars);
        }
        
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CarForRentalDTO>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCarsForRenting(string? modelname, float? rentingprice)
        {
            var cars = await _context.Car
                .Include(c=>c.Model)
                .Where(c => (c.Model.Name.Contains(modelname) || modelname == null )&&( c.RentingPrice <= rentingprice || rentingprice == null) && c.QuantityForRenting > 0)
                .Select(c=>new CarForRentalDTO(c.Id,c.Model.Name,c.FuelType,c.Manufacturer,c.RentingPrice,c.Color))
                .ToListAsync();
            _logger.LogInformation($"CarsController || Cars for renting found with the parameters {modelname} and {rentingprice}");
            return Ok(cars);
        }
        
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CarForReviewDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCarsForReview(string? filtroManufacturer, string? filtroFuelType)
        {
            var cars = await _context.Car
                .Include(c => c.Model)
                .Where(c => (c.Manufacturer.Contains(filtroManufacturer) || filtroManufacturer == null) && (c.FuelType.Contains(filtroFuelType) || (filtroFuelType == null)))
                .Select(c => new CarForReviewDTO(c.Id, c.Model.Name, c.CarClass, c.Manufacturer, c.FuelType, c.Color)) 
                .ToListAsync();
            _logger.LogInformation($"CarsController ||  Cars for review found with the parameters {filtroManufacturer} and {filtroFuelType}");
            return Ok(cars);
        }
    }
}
