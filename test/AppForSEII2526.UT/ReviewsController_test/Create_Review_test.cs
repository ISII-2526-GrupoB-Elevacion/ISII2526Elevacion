using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReviewDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ReviewsController_test
{
    public class Create_Review_test : AppForSEII25264SqliteUT
    {

        private const string UserName = "elena@navarro";
        private const string Name = "Elena";
        private const string Surname = "Navarro Martinez";

        private const string car1Model = "Ferrari La Ferrari";
        private const string car2Model = "Lamborghini Aventador";


        public Create_Review_test()
        {
            var models = new List<Model>
            {
                new Model(car1Model),
                new Model(car2Model)
            };

            var cars = new List<Car>
            {
                new Car("Sports","Red","A super fast sports car","Ferrari",150000f,2,1,180f,"4.0L","Petrol","Full Service",21,models[0]),
                new Car("Sports","Yellow","A super fast sports car","Lamborghini",160000f,2,1,190f,"4.0L","GLP","Full Service",22,models[1]),
            };

            ApplicationUser user = new ApplicationUser("1", Name, Surname, UserName);

            var review = new Review("Spain", DateTime.Today, "Experto", new List<ReviewItem>(), user);
            var reviewItems = new ReviewItem(1, "Amazing car", 4.5f, review);

            review.ReviewItems.Add(reviewItems);

            _context.Add(user);
            _context.AddRange(models);
            _context.AddRange(cars);
            _context.Add(review);
            _context.AddRange(reviewItems);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreateReviews()
        {
            var reviewNoITem = new ReviewForCreateDTO( Name, Surname, UserName,"Spain", "Experto", new List<ReviewItemDTO>());

            var reviewItems = new List<ReviewItemDTO>() { new ReviewItemDTO(car2Model, "Lamborghini","Yellow",4.5f,"Muy bueno") };

            var reviewDriverType = new ReviewForCreateDTO( Name, Surname, UserName,"Spain","Sin Carnet", reviewItems);

            var ReviewApplicationUser = new ReviewForCreateDTO( Name, Surname, "victor@lopez","Spain","Experto", reviewItems);

            var reviewCarNotExist = new ReviewForCreateDTO( Name, Surname, UserName, "Spain","Experto", new List<ReviewItemDTO>() { new ReviewItemDTO("Citroen C15", "Citroen", "Esmeralda", 1f, "Muy malo")});


            var allTests = new List<object[]>
            {             //input for create review- Error expected
                
                new object[] { reviewNoITem, "Error! You must include at least one car to be reviewed",  },
                new object[] { reviewDriverType, "Error! DriverType must be 'Novato' or 'Experto'", },
                new object[] { ReviewApplicationUser, "Error! UserName is not registered", },
                new object[] { reviewCarNotExist, "Error! The car Citroen C15 does not exist, so you cannot create a review for this car", },
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateReviews))]
        public async Task CreateReview_Error_test(ReviewForCreateDTO reviewDTO, string errorExpected)
        {
            // Arrange
            var mock = new Mock<ILogger<ReviewsController>>();
            ILogger<ReviewsController> logger = mock.Object;

            var controller = new ReviewsController(_context, logger);

            // Act
            var result = await controller.Create_Review(reviewDTO);

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            //we check that the expected error message and actual are the same
            Assert.StartsWith(errorExpected, errorActual);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateReview_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReviewsController>>();
            ILogger<ReviewsController> logger = mock.Object;

            var controller = new ReviewsController(_context, logger);

            var reviewDTO = new ReviewForCreateDTO(Name, Surname, UserName, "Spain","Experto", new List<ReviewItemDTO>()
                { new ReviewItemDTO(car2Model, "Lamborghini","Yellow",4.5f,"Muy bueno") });

            var expectedReviewDetailDTO = new ReviewDetailDTO(Name, Surname, "Spain", "Experto", DateTime.Today,
                new List<ReviewItemDTO> { new ReviewItemDTO(car2Model, "Lamborghini", "Yellow", 4.5f, "Muy bueno") });

            // Act
            var result = await controller.Create_Review(reviewDTO);

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualReviewDetailDTO = Assert.IsType<ReviewDetailDTO>(createdResult.Value);

            Assert.Equal(expectedReviewDetailDTO, actualReviewDetailDTO);

        }
    }
}