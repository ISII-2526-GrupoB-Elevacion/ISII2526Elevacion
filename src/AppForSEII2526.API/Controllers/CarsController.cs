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
        //used to enable your controller to access the database
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
        public async Task<ActionResult> GetCar_ForPurchase_DTO(string? filtroColor, string? modelo)
        {
            var cars = await _context.Car
                .Include(c => c.Model)
                .Where(c => (c.Color.Contains(filtroColor) || filtroColor == null) && (c.Model.Name.Contains(modelo) || modelo == null))
                .Select(c => new CarForPurchaseDTO(c.Id, c.Model.Name, c.Color, c.FuelType, c.Manufacturer, c.PurchasingPrice))
                .ToListAsync();
            return Ok(cars);
        }
    }
}