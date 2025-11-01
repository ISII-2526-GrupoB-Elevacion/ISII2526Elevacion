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
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(RentalDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Get_Details_Rental(int id)
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
             .Select(r => new RentalDetailDTO(r.ApplicationUser.Name, r.ApplicationUser.Surname, r.DeliveryCarDealer, r.PaymentMethod,r.StartDate,r.EndDate,r.RentingDate,r.TotalPrice,r.RentalItems
                        .Select(ri => new RentalItemDTO(ri.Car.Model.Name, ri.Car.Manufacturer, ri.Car.RentingPrice, ri.Quantity)).ToList<RentalItemDTO>()))
             .FirstOrDefaultAsync();


            if (rental == null)
            {
                _logger.LogError($"Error: Rental with id {id} does not exist");
                return NotFound();
            }


            return Ok(rental);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(RentalDetailDTO),(int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> Create_Rental(RentalForCreateDTO rentalForCreate)
        {
            if (rentalForCreate.StartDate<= DateTime.Today)
                ModelState.AddModelError("RentalDateFrom", "Error! Your rental date must start later than today");

            if (rentalForCreate.StartDate >= rentalForCreate.EndDate)
                ModelState.AddModelError("RentalDateFrom&RentalDateTo", "Error! Your rental must end later than it starts");

            if (rentalForCreate.RentalItems.Count==0)
                ModelState.AddModelError("RentalItems", "Error! You must include at least one car to be rented");

            //if (!_context.ApplicationUser.Any(au=>au.UserName == rentalFromCreate.CustomerUserName)) Es lo mismo que lo de abajo pero de otra forma
            var user = _context.ApplicationUser.FirstOrDefault(au => au.UserName == rentalForCreate.UserName);
            if (user == null)
                ModelState.AddModelError("RentalAplicationUser", "Error! UserName is not registered");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var carModels = rentalForCreate.RentalItems.Select(ri=>ri.Model).ToList<string>();

            var cars = _context.Car.Include(r => r.RentalItems)
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
            Rental rental = new Rental(rentalForCreate.DeliveryCarDealer,rentalForCreate.EndDate,rentalForCreate.PaymentMethod,rentalForCreate.RentingDate,rentalForCreate.StartDate,new List<RentalItem>(),user);

            rental.TotalPrice = 0;
            var numDays = (rental.EndDate- rental.StartDate).TotalDays;

            foreach (var item in rentalForCreate.RentalItems)
            {
                var car = cars.FirstOrDefault(c => c.Name == item.Model);
                //we must check that there is enough quantity to be rented in the database
                if ((car == null) || ((car.NumberOfRentedItems+item.Quantity) >= car.QuantityForRenting))
                {
                    ModelState.AddModelError("RentalItems", $"Error! Car '{item.Model}' is not available for being rented from {rentalForCreate.StartDate.ToShortDateString()} to {rentalForCreate.EndDate.ToShortDateString()}");
                }
                else
                {
                    // rental does not exist in the database yet and does not have a valid Id, so we must relate rentalitem to the object rental
                    rental.RentalItems.Add(new RentalItem(car.Id,item.Quantity,rental));
                    item.RentingPrice = car.RentingPrice;
                    rental.TotalPrice += (float)(car.RentingPrice * item.Quantity * numDays);
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(rental);

            try
            {
                //we store in the database both rental and its rentalitems
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Rental", $"Error! There was an error while saving your rental, plese, try again later");
                return Conflict("Error" + ex.Message);

            }

            var rentalDetail = new RentalDetailDTO(rental.ApplicationUser.Name, rental.ApplicationUser.Surname, rental.DeliveryCarDealer, rental.PaymentMethod,rental.StartDate,rental.EndDate,rental.RentingDate,rental.TotalPrice,rentalForCreate.RentalItems);

            return CreatedAtAction("Get_Details_Rental", new { id = rental.Id }, rentalDetail);
        }

    }
}
