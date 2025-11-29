using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.CarDTO;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.CarsController_test
{
    public class GetCars_ForReview_test : AppForSEII25264SqliteUT
    {
        public GetCars_ForReview_test()
        {
            var models = new List<Model>() //creo una minibase de datos
            {
                new Model("Ferrari La Ferrari"),
                new Model("Lamborghini Aventador"),
                new Model("Porsche 911"),
                new Model("McLaren P1")
            };

            var car = new List<Car>()
            {
                new Car("Sports","Red","A super fast sports car","Ferrari",150000f,2,1,180f,"4.0L","Petrol","Full Service",21,models[0]),
                new Car("Sports","Yellow","A super fast sports car","Lamborghini",160000f,2,1,190f,"4.0L","GLP","Full Service",22,models[1]),
                new Car("Sports","Black","A super fast sports car","Porsche",140000f,2,1,170f,"3.8L","Petrol","Full Service",20,models[2]),
                new Car("Sports","Orange","A super fast sports car","McLaren",155000f,2,1,185f,"4.0L","GLP","Full Service",21,models[3])
            };
            ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martinez", "elena@uclm.es"); //creo un usuario ficticio

            var review = new Review("Spain", DateTime.Now, "Experto", new List<ReviewItem>(), user); //creo una review y una reviewItem ficticia
            var reviewItems = new ReviewItem(1, "Amazing car", 4.5f, review);

            review.ReviewItems.Add(reviewItems); //lo añado todo a la base de datos

            _context.Add(user);
            _context.AddRange(models);
            _context.AddRange(car);
            _context.Add(review);
            _context.AddRange(reviewItems);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetCarsForReview_OK()
        {
            var carDTOs = new List<CarForReviewDTO>() //los coches con los que trabajaré
            {
                new CarForReviewDTO(1,"Ferrari La Ferrari","Sports","Ferrari","Petrol","Red"),
                new CarForReviewDTO(2,"Lamborghini Aventador","Sports","Lamborghini","GLP","Yellow"),
                new CarForReviewDTO(3,"Porsche 911","Sports","Porsche","Petrol","Black"),
                new CarForReviewDTO(4,"McLaren P1","Sports","McLaren","GLP","Orange")
            };
            var carDTOsTC1 = new List<CarForReviewDTO>() { carDTOs[0], carDTOs[1], carDTOs[2], carDTOs[3] }; //los resultados que espero obtener con las pruebas
            var carDTOsTC2 = new List<CarForReviewDTO>() { carDTOs[0] };
            var carDTOsTC3 = new List<CarForReviewDTO>() { carDTOs[1], carDTOs[3] };
            var carDTOsTC4 = new List<CarForReviewDTO>() { carDTOs[2] };

            var allTest = new List<object[]> //casos de prueba
            {
                new object[] { null, null, carDTOsTC1 },
                new object[] { "Ferrari", null, carDTOsTC2 },
                new object[] { null, "GLP", carDTOsTC3 },
                new object[] { "Porsche", "Petrol", carDTOsTC4 },

            };

            return allTest;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetCarsForReview_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCarsForReview_OK_test(string? filtroManufacturer, string? filtroFuelType, IList<CarForReviewDTO> expectedCars)
        {
            // Arrange
            var mock = new Mock<ILogger<CarsController>>();
            ILogger<CarsController> logger = mock.Object;
            var controller = new CarsController(_context, logger);
            // Act
            var result = await controller.GetCarsForReview(filtroManufacturer, filtroFuelType); //filtro por el manufacturer y el fueltype
            //Assert
            //we check that the response type is OK 
            var okResult = Assert.IsType<OkObjectResult>(result);
            //and obtain the list of cars
            var carDTOsActual = Assert.IsType<List<CarForReviewDTO>>(okResult.Value);
            Assert.Equal(expectedCars, carDTOsActual); //si lo esperado es igual a lo actual, me devuelve el OK
        }


        }
    }   

