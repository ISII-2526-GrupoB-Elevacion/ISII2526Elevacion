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
    public class GetCars_ForReview_DTO_test : AppForSEII25264SqliteUT
    {
        public GetCars_ForReview_DTO_test()
        {
            var models = new List<Model>()
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
            ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martinez", "elena@uclm.es");

            var review = new Review("Spain", DateTime.Now, "Experto", new List<ReviewItem>(), user);
            var reviewItems = new ReviewItem(1, "Amazing car", 4.5f, review);

            review.ReviewItems.Add(reviewItems);

            _context.Add(user);
            _context.AddRange(models);
            _context.AddRange(car);
            _context.Add(review);
            _context.AddRange(reviewItems);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetCarsForReview_OK()
        {
            var carDTOs = new List<CarForReviewDTO>()
            {
                new CarForReviewDTO(1,"Ferrari La Ferrari","Sports","Ferrari","Petrol","Red"),
                new CarForReviewDTO(2,"Lamborghini Aventador","Sports","Lamborghini","GLP","Yellow"),
                new CarForReviewDTO(3,"Porsche 911","Sports","Porsche","Petrol","Black"),
                new CarForReviewDTO(4,"McLaren P1","Sports","McLaren","GLP","Orange")
            };
            var carDTOsTC1 = new List<CarForReviewDTO>() { carDTOs[0], carDTOs[1], carDTOs[2], carDTOs[3] };
            var carDTOsTC2 = new List<CarForReviewDTO>() { carDTOs[0] };
            var carDTOsTC3 = new List<CarForReviewDTO>() { carDTOs[1], carDTOs[3] };
            var carDTOsTC4 = new List<CarForReviewDTO>() { carDTOs[2] };

            var allTest = new List<object[]>
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
            var controller = new CarsController(_context, null);
            // Act
            var result = await controller.GetCars_ForReview(filtroManufacturer, filtroFuelType);
            //Assert
            //we check that the response type is OK 
            var okResult = Assert.IsType<OkObjectResult>(result);
            //and obtain the list of cars
            var carDTOsActual = Assert.IsType<List<CarForReviewDTO>>(okResult.Value);
            Assert.Equal(expectedCars, carDTOsActual);
        }


        }
    }   

