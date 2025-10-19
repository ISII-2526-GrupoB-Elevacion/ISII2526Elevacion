using AppForSEII2526.API.DTOs.PurchaseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PurchasesController> _logger;

        public PurchasesController(ApplicationDbContext context, ILogger<PurchasesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Get_Details_Purchase_DTO(int id)
        {
            if (_context.Purchase == null)
            {
                _logger.LogError("Error: Purchase table does not exist");
                return NotFound();
            }

            var purchase = await _context.Purchase
             .Where(p => p.Id == id)
                 .Include(p => p.ApplicationUser) //join table ApplicationUser
                 .Include(p => p.PurchaseItems) //join table PurchaseItem
                    .ThenInclude(pi => pi.Car) //then join table Car
                        .ThenInclude(c => c.Model) //then join table Model     
             .Select(p => new PurchaseDetailDTO(p.ApplicationUser.Name, p.ApplicationUser.Surname, p.PurchasingDate, p.PurchasingPrice, p.PurchaseItems
                        .Select(pi => new PurchaseItemDTO(pi.Car.Model.Name, pi.Quantity, pi.Car.PurchasingPrice, pi.Car.Color)).ToList<PurchaseItemDTO>()))
             .FirstOrDefaultAsync();


            if (purchase == null)
            {
                _logger.LogError($"Error: Purchase with id {id} does not exist");
                return NotFound();
            }


            return Ok(purchase);
        }
    }
}
