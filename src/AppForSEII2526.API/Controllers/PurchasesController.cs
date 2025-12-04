using AppForSEII2526.API.DTOs.PurchaseDTO;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
            _logger.LogInformation("Controller 'PurchasesController' inicializado");
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetDetailsPurchase(int id)
        {
            if (_context.Purchase == null) //si no existe la tabla purchase devuelve error NotFound()
            {
                _logger.LogError("PurchasesController || Error: Purchase table does not exist");
                return NotFound();
            }

            var purchase = await _context.Purchase //si existe purchase, se va haciendo joins entre distintas tablas hasta juntar e imprimir todos los atributos necesarios
             .Where(p => p.Id == id)
                 .Include(p => p.ApplicationUser) //join table ApplicationUser
                 .Include(p => p.PurchaseItems) //join table PurchaseItem
                    .ThenInclude(pi => pi.Car) //then join table Car
                        .ThenInclude(c => c.Model) //then join table Model     
             .Select(p => new PurchaseDetailDTO(p.ApplicationUser.Name, p.ApplicationUser.Surname, p.ApplicationUser.UserName, p.DeliveryCarDealer, p.PurchasingDate, p.PurchasingPrice, p.PurchaseItems
                        .Select(pi => new PurchaseItemDTO(pi.Car.Model.Name, pi.Quantity, pi.Car.PurchasingPrice, pi.Car.Color)).ToList<PurchaseItemDTO>()))
             .FirstOrDefaultAsync();


            if (purchase == null) //si no he obtenido ninguna compra con el id que le he pasado, entonces devuelve error NotFound()
            {
                _logger.LogError($"PurchasesController || Error: Purchase with id {id} does not exist");
                return NotFound();
            }


            return Ok(purchase);
        }

        [HttpPost] //operación de creación
        [Route("[action]")] //operación de tipo acción
        [ProducesResponseType(typeof(PurchaseDetailDTO), (int)HttpStatusCode.Created)] //devuelve OK cuando consigue meter en la base de datos el código
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)] //devuelve BadRequest cuando hay un error durante la comprobación de la petición
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)] //devuelve Conflict cuando hay un error al añadir a la base de datos
        public async Task<ActionResult> CreatePurchase(PurchaseForCreateDTO purchaseForCreate)
        {
            if (purchaseForCreate.PurchaseItems.Count == 0) //compruebo que he seleccionado algún coche para comprar.
            {
                ModelState.AddModelError("PurchaseItems", "Error! You must include at least one car to be purchased");
                _logger.LogError("PurchasesController || Error! You must include at least one car to be purchased");
            }

            var user = _context.ApplicationUser.FirstOrDefault(au => au.UserName == purchaseForCreate.UserName); //compruebo que el usuario que compra existe en la base de datos
            if (user == null)
            {
                ModelState.AddModelError("PurchaseApplicationUser", "Error! UserName is not registered");
                _logger.LogError("PurchasesController || Error! UserName is not registered");
            }

            if (ModelState.ErrorCount > 0) //si tengo algún error acumulado, devuelve BadRequest
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var purchaseCars = purchaseForCreate.PurchaseItems.Select(pi => pi.Model).ToList<string>(); //saco todos los modelos de los coches que he pedido para comprar

            var cars = _context.Car.Include(c => c.PurchaseItems) //cojo de la tabla coche los atributos necesarios de filas que ya están añadidas para después llevar a cabo distintas comprobaciones de compatibilidad a la hora de comprobar la posibilidad de añadir una nueva compra a la tabla Purchase
                .ThenInclude(pi => pi.Purchase)
                .Include(c => c.Model)
                .Where(c => purchaseCars.Contains(c.Model.Name))
                .Select(c => new
                {
                    c.Id,
                    c.Model.Name,
                    c.QuantityForPurchasing,
                    c.PurchasingPrice,
                    NumberOfPurchaseItems = c.PurchaseItems.Sum(pi => pi.Quantity) //nº de coches que ya están vendidos del conjunto disponible para la compra
                })
                .ToList();

            Purchase purchase = new Purchase(purchaseForCreate.DeliveryCarDealer, purchaseForCreate.PaymentMethod, DateTime.Today, new List<PurchaseItem>(), user); //creo la compra según los parámetros del DTO que le he pasado al método
            purchase.PurchasingPrice = 0; //establezco el precio de la compra a 0, porque luego ese precio lo voy a ir calculando más abajo, es decir no se va a quedar así

            foreach (var item in purchaseForCreate.PurchaseItems)
            {
                var car = cars.FirstOrDefault(c => c.Name == item.Model); //saco el modelo de coche que he pedido para comprar

                if (car == null) //si el modelo que he pedido no existe, añado un error
                {
                    ModelState.AddModelError("PurchaseItems", $"Error! The car {item.Model} is not for sale at the dealership");
                    _logger.LogError($"PurchasesController || Error! The car {item.Model} is not for sale at the dealership");
                }
                else if ((car.NumberOfPurchaseItems + item.Quantity) > car.QuantityForPurchasing) //si el nº de coches que ya están vendidos más los que yo quiero comprar superan las cantidades de aquellos que hay disponibles para comprar, devuelve un error. Es decir compruebo el Stock
                {
                    ModelState.AddModelError("PurchaseItems", $"Error! There are not enough units available to purchase the car {item.Model}");
                    _logger.LogError($"PurchasesController || Error! There are not enough units available to purchase the car {item.Model}");
                }
                else if (item.Quantity==2 && string.IsNullOrEmpty(item.Description))
                {
                    ModelState.AddModelError("Purchase Items", $"Error! Estás comprando demasiados coches sin descripción");
                }
                else //una vez todos los criterios han sido cumplidos, acabo de rellenar la compra con los atributos necesarios y voy calculando el precio total de la compra según los atributos de la tabla coche. Multiplico el precio de un coche por la cantidad de coches que quiero comprar
                {
                    purchase.PurchaseItems.Add(new PurchaseItem(car.Id, item.Quantity, purchase));
                    item.PurchasingPrice = car.PurchasingPrice;
                    purchase.PurchasingPrice += (car.PurchasingPrice * item.Quantity);
                }
            }

            if (ModelState.ErrorCount > 0) //si tengo algún error acumulado devuelvo error BadRequest()
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(purchase); //añado la propia compra a la base de datos del programa

            try
            {
                await _context.SaveChangesAsync(); //guardo los cambios
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Purchase", $"Error! There was an error while saving your purchase, plese, try again later");
                _logger.LogError($"PurchasesController || Error! {ex.Message}");
                return Conflict("Error" + ex.Message); //si ocurrió algún error inesperado, devuelvo un error Conflict()
            }

            var purchaseDetail = new PurchaseDetailDTO(purchase.ApplicationUser.Name, purchase.ApplicationUser.Surname, purchase.ApplicationUser.UserName, purchase.DeliveryCarDealer, purchase.PurchasingDate,
                purchase.PurchasingPrice, purchaseForCreate.PurchaseItems); //monto los detalles de la compra que acabo de realizar para posteriormente mostrarlos al cliente

            _logger.LogInformation($"PurchasesController || La compra {purchase.Id} se ha realizado correctamente");
            return CreatedAtAction("GetDetailsPurchase", new { id = purchase.Id }, purchaseDetail); //devuelvo un return de CreatedAction indicando al usuario que el coche ya ha sido creado, a la misma vez que paso la referencia de la compra (id.) al método de detalles a la vez que la propia información que tienen que sacar por pantalla
        }
    }
}
