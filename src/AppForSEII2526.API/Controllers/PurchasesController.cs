using AppForSEII2526.API.DTOs.PurchaseDTO;
using AppForSEII2526.API.Models;
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
        public async Task<ActionResult> Get_Details_Purchase(int id)
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
             .Select(p => new PurchaseDetailDTO(p.ApplicationUser.Name, p.ApplicationUser.Surname, p.DeliveryCarDealer, p.PurchasingDate, p.PurchasingPrice, p.PurchaseItems
                        .Select(pi => new PurchaseItemDTO(pi.Car.Model.Name, pi.Quantity, pi.Car.PurchasingPrice, pi.Car.Color)).ToList<PurchaseItemDTO>()))
             .FirstOrDefaultAsync();


            if (purchase == null)
            {
                _logger.LogError($"Error: Purchase with id {id} does not exist");
                return NotFound();
            }


            return Ok(purchase);
        }

        [HttpPost] //operación de creación
        [Route("[action]")] //operación de tipo acción
        [ProducesResponseType(typeof(PurchaseDetailDTO), (int)HttpStatusCode.Created)] //devuelve OK cuando consigue meter en la base de datos el código
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)] //devuelve BadRequest cuando hay un error durante la comprobación de la petición
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)] //devuelve Conflict cuando hay un error al añadir a la base de datos
        public async Task<ActionResult> Create_Purchase(PurchaseForCreateDTO purchaseForCreate)
        {
            if (purchaseForCreate.PurchaseItems.Count == 0) //compruebo que he seleccionado algún coche para comprar.
            {
                ModelState.AddModelError("PurchaseItems", "Error! You must include at least one car to be purchased");
            }

            var user = _context.ApplicationUser.FirstOrDefault(au => au.UserName == purchaseForCreate.UserName); //compruebo que el usuario que compra existe en la base de datos
            if (user == null)
            {
                ModelState.AddModelError("PurchaseApplicationUser", "Error! UserName is not registered");
            }

            if (ModelState.ErrorCount > 0) //si tengo algún error acumulado, devuelve BadRequest
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var purchaseCars = purchaseForCreate.PurchaseItems.Select(pi => pi.Model).ToList<string>();

            var cars = _context.Car.Include(c => c.PurchaseItems)
                .ThenInclude(pi => pi.Purchase)
                .Include(c => c.Model)
                .Where(c => purchaseCars.Contains(c.Model.Name))
                .Select(c => new
                {
                    c.Id,
                    c.Model.Name,
                    c.QuantityForPurchasing,
                    c.PurchasingPrice,
                    NumberOfPurchaseItems = c.PurchaseItems.Sum(pi => pi.Quantity)
                })
                .ToList();

            Purchase purchase = new Purchase(purchaseForCreate.DeliveryCarDealer, purchaseForCreate.PaymentMethod, DateTime.Now, new List<PurchaseItem>(), user);
            purchase.PurchasingPrice = 0;

            foreach (var item in purchaseForCreate.PurchaseItems)
            {
                var car = cars.FirstOrDefault(c => c.Name == item.Model);

                if (car == null)
                {
                    ModelState.AddModelError("PurchaseItems", $"Error! The car {item.Model} is not for sale at the dealership");
                }
                else if ((car.NumberOfPurchaseItems + item.Quantity) > car.QuantityForPurchasing)
                {
                    ModelState.AddModelError("PurchaseItems", $"Error! There are not enough units available to purchase the car {item.Model}");
                }
                else
                {
                    purchase.PurchaseItems.Add(new PurchaseItem(car.Id, item.Quantity, purchase));
                    item.PurchasingPrice = car.PurchasingPrice;
                    purchase.PurchasingPrice += (car.PurchasingPrice * item.Quantity);
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(purchase);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Purchase", $"Error! There was an error while saving your purchase, plese, try again later");
                return Conflict("Error" + ex.Message);
            }

            var purchaseDetail = new PurchaseDetailDTO(purchase.ApplicationUser.Name, purchase.ApplicationUser.Surname, purchase.DeliveryCarDealer, purchase.PurchasingDate,
                purchase.PurchasingPrice, purchaseForCreate.PurchaseItems);

            return CreatedAtAction("Get_Details_Purchase", new { id = purchase.Id }, purchaseDetail);
        }
    }
}
