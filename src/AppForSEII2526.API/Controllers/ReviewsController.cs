using AppForSEII2526.API.DTOs.ReviewDTO;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(ApplicationDbContext context, ILogger<ReviewsController> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogInformation("Controller 'ReviewsController' inicializado");
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReviewDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Get_Details_Review(int id)
        {
            if (_context.Review == null) //si no encuentra la tabla Review lanzo un error
            {
                _logger.LogError("ReviewsController || Error: Review table does not exist");
                return NotFound();
            }

            var review = await _context.Review //voy haciendo joins hasta tener todos los datos que se me piden
             .Where(p => p.Id == id)
                 .Include(p => p.ApplicationUser) //join table ApplicationUser
                 .Include(p => p.ReviewItems) //join table ReviewItem
                    .ThenInclude(pi => pi.Car) //then join table Car
                        .ThenInclude(c => c.Model) //then join table Model     
             .Select(p => new ReviewDetailDTO(p.ApplicationUser.Name, p.ApplicationUser.Surname, p.Country, p.DriverType, p.Created, p.ReviewItems
                        .Select(pi => new ReviewItemDTO(pi.Car.Model.Name, pi.Car.Manufacturer, pi.Car.Color, pi.Rating, pi.Description)).ToList<ReviewItemDTO>()))
             .FirstOrDefaultAsync();


            if (review == null) //si el id que le paso no corresponde a ninguna review lanzo un error
            {
                _logger.LogError($"ReviewsController || Error: Review with id {id} does not exist");
                return NotFound();
            }


            return Ok(review);
        }

        [HttpPost] //operación de creación
        [Route("[action]")] //operación de tipo acción
        [ProducesResponseType(typeof(ReviewDetailDTO), (int)HttpStatusCode.Created)] //devuelve OK cuando consigue meter en la base de datos el código
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)] //devuelve BadRequest cuando hay un error durante la comprobación de la petición
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)] //devuelve Conflict cuando hay un error al añadir a la base de datos
        public async Task<ActionResult> Create_Review(ReviewForCreateDTO reviewForCreate)
        {
            if (reviewForCreate.ReviewItems.Count == 0) //compruebo que he seleccionado algún coche para comprar.
            {
                ModelState.AddModelError("ReviewItems", "Error! You must include at least one car to be reviewed");
                _logger.LogError($"ReviewsController || Error! You must include at least one car to be reviewed");
            }

            if (reviewForCreate.DriverType != "Novato" && reviewForCreate.DriverType != "Experto") //compruebo que el tipo de conductor es correcto
            {
                ModelState.AddModelError("DriverType", "Error! DriverType must be 'Novato' or 'Experto'");
                _logger.LogError($"ReviewsController || Error! DriverType must be 'Novato' or 'Experto'");
            }

            var user = _context.ApplicationUser.FirstOrDefault(au => au.UserName == reviewForCreate.UserName); //compruebo que el usuario que compra existe en la base de datos
            if (user == null) //si el usuario no existe lanzo un error
            {
                ModelState.AddModelError("ReviewApplicationUser", "Error! UserName is not registered");
                _logger.LogError($"ReviewsController || Error! UserName is not registered'");
            }

            if (ModelState.ErrorCount > 0) //si tengo algún error acumulado, devuelve BadRequest
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var reviewCars = reviewForCreate.ReviewItems.Select(pi => pi.Model).ToList<string>(); //hago una lista con los modelos de los coches que le he pasado

            var cars = _context.Car //me saca los coches con los datos que se me han pedido
                .Include(c => c.Model)
                .Where(c => reviewCars.Contains(c.Model.Name))
                .Select(c => new
                {
                    c.Id,
                    c.Model.Name
                })
                .ToList();

            Review review = new Review(reviewForCreate.Country, DateTime.Today,reviewForCreate.DriverType, new List<ReviewItem>(), user); //creo una nueva review

            foreach (var item in reviewForCreate.ReviewItems)
            {
                var car = cars.FirstOrDefault(c => c.Name == item.Model); //saco el primer coche que me encuentra, sacando su nombre y modelo

                if (car == null) //si el nombre o modelo no existen me salta un error
                {
                    ModelState.AddModelError("ReviewItems", $"Error! The car {item.Model} does not exist, so you cannot create a review for this car");
                    _logger.LogError($"ReviewsController || Error! The car {item.Model} does not exist, so you cannot create a review for this car");
                }
                else
                {
                    review.ReviewItems.Add(new ReviewItem(car.Id, item.Description, item.Rating, review)); //si todo es correcto, añade a reviewItems una nueva reviewItem
                }
            }

            if (ModelState.ErrorCount > 0) //si tiene algún error finaliza
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(review); //añade a la base de datos la review

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Review", $"Error! There was an error while saving your review, plese, try again later");
                _logger.LogError($"ReviewsController || Error! {ex.Message}");
                return Conflict("Error" + ex.Message);
            }

            var reviewDetail = new ReviewDetailDTO(review.ApplicationUser.Name,review.ApplicationUser.Surname, review.Country, review.DriverType, review.Created, reviewForCreate.ReviewItems);

            _logger.LogInformation($"ReviewsController || La reseña {review.Id} se ha guardado correctamente");
            return CreatedAtAction("Get_Details_Review", new { id = review.Id }, reviewDetail);
        }
    }
}
