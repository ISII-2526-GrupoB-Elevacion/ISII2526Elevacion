using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReviewDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ReviewsController_test
{
    public class Get_Details_Review_test : AppForSEII25264SqliteUT
    {
        public Get_Details_Review_test()
        {
            var models = new List<Model>() //me creo una minibase de datos
            {
                new Model("Ferrari La Ferrari"),
                new Model("Lamborghini Aventador"),
            };

            var car = new List<Car>()
            {
                new Car("Sports","Red","A super fast sports car","Ferrari",150000f,2,1,180f,"4.0L","Petrol","Full Service",21,models[0]),
                new Car("Sports","Yellow","A super fast sports car","Lamborghini",160000f,2,1,190f,"4.0L","GLP","Full Service",22,models[1]),
            };

            ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martinez", "elena@uclm.es"); //creo un usuario ficticio

            var review = new Review("Spain", DateTime.Today, "Experto", new List<ReviewItem>(), user); //creo una review y una reviewItem ficticia
            var reviewItems = new ReviewItem(1, "Amazing car", 4.5f, review);



            review.ReviewItems.Add(reviewItems);

            _context.Add(user); //los añado a la base de datos
            _context.AddRange(models);
            _context.AddRange(car);
            _context.Add(review);
            _context.AddRange(reviewItems);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDetailsReview_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReviewsController>>();
            ILogger<ReviewsController> logger = mock.Object;

            var controller = new ReviewsController(_context, logger);

            // Act
            var result = await controller.GetDetailsReview(0);

            //Assert
            //we check that the response type is OK and obtain the list of cars
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetDetailsReview_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReviewsController>>();
            ILogger<ReviewsController> logger = mock.Object;
            var controller = new ReviewsController(_context, logger);

            //me creo la review esperada
            var expectedReview = new ReviewDetailDTO(1, "Elena", "Navarro Martinez", "elena@uclm.es", "Spain", "Experto", DateTime.Today, new List<ReviewItemDTO>());
            expectedReview.ReviewItems.Add(new ReviewItemDTO("Ferrari La Ferrari", "Ferrari", "Red", 4.5f, "Amazing car"));

            // Act 
            var result = await controller.GetDetailsReview(1);

            //Assert
            //we check that the response type is OK and obtain the rental
            var okResult = Assert.IsType<OkObjectResult>(result);
            var reviewDTOActual = Assert.IsType<ReviewDetailDTO>(okResult.Value);
            var eq = expectedReview.Equals(reviewDTOActual);

            //compruebo que la esperada y la actual sean la misma
            Assert.Equal(expectedReview, reviewDTOActual);

        }
    }
}
