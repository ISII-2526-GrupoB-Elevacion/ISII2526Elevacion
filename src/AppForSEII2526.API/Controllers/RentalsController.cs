using AppForSEII2526.API.DTOs.RentalDTO;
using AppForSEII2526.API.Models;
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
            _logger.LogInformation("Controller 'RentalsController' inicializado");
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(RentalDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetDetailsRental(int id)
        {
            if (_context.Rental == null) //compruebo si no existe la tabla Rental y devuelvo un error
            {
                _logger.LogError("RentalsController || Error: Rental table does not exist");
                return NotFound();
            }

            var rental = await _context.Rental //voy haciendo joins con el resto de las tablas hasta conseguir los datos necesarios a mostrar
             .Where(r => r.Id == id)
                 .Include(r => r.ApplicationUser) //join table ApplicationUser
                 .Include(r => r.RentalItems) //join table RentalItem
                    .ThenInclude(ri => ri.Car) //then join table Car
                        .ThenInclude(c => c.Model) //then join table Model     
             .Select(r => new RentalDetailDTO(r.ApplicationUser.Name, r.ApplicationUser.Surname, r.DeliveryCarDealer, r.PaymentMethod,r.StartDate,r.EndDate,r.RentingDate,r.TotalPrice,r.RentalItems
                        .Select(ri => new RentalItemDTO(ri.Car.Model.Name, ri.Car.Manufacturer, ri.Car.RentingPrice, ri.Quantity)).ToList<RentalItemDTO>()))
             .FirstOrDefaultAsync();


            if (rental == null) //si no consigo los datos muestro error (no existe ese alquiler)
            {
                _logger.LogError($"RentalsController || Error: Rental with id {id} does not exist");
                return NotFound();
            }


            return Ok(rental);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(RentalDetailDTO),(int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateRental(RentalForCreateDTO rentalForCreate)
        {
            if (rentalForCreate.StartDate<= DateTime.Today) //compruebo si la fecha de comienzo es anterior a hoy
                ModelState.AddModelError("RentalDateFrom", "Error! Your rental date must start later than today");
                _logger.LogError($"RentalsController || Error! Your rental date must start later than today");

            if (rentalForCreate.StartDate >= rentalForCreate.EndDate) //compruebo si la fecha de comienzo es posterior a la fecha de fin
                ModelState.AddModelError("RentalDateFrom&RentalDateTo", "Error! Your rental must end later than it starts");
                _logger.LogError($"RentalsController || Error! Your rental must end later than it starts");

            if (rentalForCreate.RentalItems.Count==0) //compruebo si hay items para alquilar
                ModelState.AddModelError("RentalItems", "Error! You must include at least one car to be rented");
                _logger.LogError($"RentalsController || Error! You must include at least one car to be rented");

            if (!rentalForCreate.DeliveryCarDealer.Contains("Calle"))
            {
                ModelState.AddModelError("DeliveryCarDealer", "Error! La dirección de envío debe empezar por la palabra Calle");
                _logger.LogError($"RentalsController || Error! La dirección de envío debe empezar por la palabra Calle");
            }

            //if (!_context.ApplicationUser.Any(au=>au.UserName == rentalFromCreate.CustomerUserName)) Es lo mismo que lo de abajo pero de otra forma
            var user = _context.ApplicationUser.FirstOrDefault(au => au.UserName == rentalForCreate.UserName);
            if (user == null) //compruebo si el usuario existe
                ModelState.AddModelError("RentalAplicationUser", "Error! UserName is not registered");
                _logger.LogError($"RentalsController || Error! UserName is not registered");

            if (ModelState.ErrorCount > 0) //si hay algún error lo devuelvo
                return BadRequest(new ValidationProblemDetails(ModelState));

            var carModels = rentalForCreate.RentalItems.Select(ri=>ri.Model).ToList<string>(); //para los coches seleccionados guardo sus modelos en una lista

            var cars = _context.Car.Include(r => r.RentalItems) //para cada coche seleccionado obtengo los datos necesarios
                .ThenInclude(ri => ri.Car)
                .ThenInclude(c => c.Model)
                .Where(r => carModels.Contains(r.Model.Name))
                .Select(c => new
                {
                    c.Id,
                    c.Model.Name,
                    c.RentingPrice,
                    c.QuantityForRenting,
                    NumberOfRentedItems = c.RentalItems.Count(ri => ri.Rental.StartDate <= rentalForCreate.EndDate
                            && ri.Rental.EndDate >= rentalForCreate.StartDate)
                }
                ).ToList();
            Rental rental = new Rental(rentalForCreate.DeliveryCarDealer,rentalForCreate.EndDate,rentalForCreate.PaymentMethod,rentalForCreate.RentingDate,rentalForCreate.StartDate,new List<RentalItem>(),user); //creo el alquiler asociado al usuario

            rental.TotalPrice = 0; //asocio el precio total a 0, para luego poder calcular el precio bien
            var numDays = (rental.EndDate- rental.StartDate).TotalDays; //calculo el número de días de alquiler

            foreach (var item in rentalForCreate.RentalItems)
            { 
                var car = cars.FirstOrDefault(c => c.Name == item.Model);
                //we must check that there is enough quantity to be rented in the database
                if ((car == null) || ((car.NumberOfRentedItems+item.Quantity) >= car.QuantityForRenting)) //para cada coche compruebo si no se puede alquilar o si no existe, y si cumple algunos de las 2 condiciones devuelve error
                {
                    ModelState.AddModelError("RentalItems", $"Error! Car '{item.Model}' is not available for being rented from {rentalForCreate.StartDate.ToShortDateString()} to {rentalForCreate.EndDate.ToShortDateString()}");
                    _logger.LogError($"RentalsController || Error! Car '{item.Model}' is not available for being rented from {rentalForCreate.StartDate.ToShortDateString()} to {rentalForCreate.EndDate.ToShortDateString()}");
                }
                else //calculo el precio y añado el alquiler
                {
                    // rental does not exist in the database yet and does not have a valid Id, so we must relate rentalitem to the object rental
                    rental.RentalItems.Add(new RentalItem(car.Id,item.Quantity,rental));
                    item.RentingPrice = car.RentingPrice;
                    rental.TotalPrice += (float)(car.RentingPrice * item.Quantity * numDays);
                }
            }

            if (ModelState.ErrorCount > 0) //si hay algún error lo devuelvo
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(rental); //añado el alquiler a la base de datos

            try
            {
                //we store in the database both rental and its rentalitems
                await _context.SaveChangesAsync(); //compruebo si se pueden guardar bien los datos y si hay algún error lo devuelvo
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Rental", $"Error! There was an error while saving your rental, plese, try again later");
                _logger.LogError($"RentalsController || Error! {ex.Message}");
                return Conflict("Error" + ex.Message);

            }

            //creo el detalle del alquiler para mostrarlo por pantalla
            var rentalDetail = new RentalDetailDTO(rental.ApplicationUser.Name, rental.ApplicationUser.Surname, rental.DeliveryCarDealer, rental.PaymentMethod,rental.StartDate,rental.EndDate,rental.RentingDate,rental.TotalPrice,rentalForCreate.RentalItems);

            _logger.LogInformation($"RentalsController || El alquiler {rental.Id} se ha creado correctamente");
            return CreatedAtAction("GetDetailsRental", new { id = rental.Id }, rentalDetail); //devuelvo que se ha creado el alquiler y muestro el detalle
        }

    }
}
