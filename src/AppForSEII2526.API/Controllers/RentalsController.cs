using AppForSEII2526.API.DTOs.RentalDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RentalsController> _logger;

        public RentalsController(ApplicationDbContext context, ILogger<RentalsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(RentalDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Get_Details_Rental_DTO(int id)
        {
            if (_context.Rental == null)
            {
                _logger.LogError("Error: Rental table does not exist");
                return NotFound();
            }

            var rental = await _context.Rental
             .Where(r => r.Id == id)
                 .Include(r => r.ApplicationUser) //join table ApplicationUser
                 .Include(r => r.RentalItems) //join table RentalItem
                    .ThenInclude(ri => ri.Car) //then join table Car
                        .ThenInclude(c => c.Model) //then join table Model     
             .Select(r => new RentalDetailDTO(r.ApplicationUser.Name, r.ApplicationUser.Surname,r.PaymentMethod,r.StartDate,r.EndDate,r.RentingDate,r.TotalPrice,r.RentalItems
                        .Select(ri => new RentalItemDTO(ri.Car.Model.Name, ri.Car.Manufacturer, ri.Car.RentingPrice, ri.Quantity)).ToList<RentalItemDTO>()))
             .FirstOrDefaultAsync();


            if (rental == null)
            {
                _logger.LogError($"Error: Rental with id {id} does not exist");
                return NotFound();
            }


            return Ok(rental);
        }
    }
}
