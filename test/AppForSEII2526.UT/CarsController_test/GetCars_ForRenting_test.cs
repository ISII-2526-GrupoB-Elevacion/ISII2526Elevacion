using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.CarDTO;


namespace AppForSEII2526.UT.CarsController_test
{
    public class GetCars_ForRenting_test : AppForSEII25264SqliteUT
    {
        public GetCars_ForRenting_test()
        {
            var models = new List<Model>
            {
                new Model("Citroen c15"),
                new Model("Ford F150"),
                new Model("Ferrari laFerrari"),
                new Model("Peugot 305")
            };

            var cars = new List<Car>
            {
                new Car("Furgoneta", "Blanco", "La furgoneta", "Citroen", 5000f, 10, 5, 50f, "1.4L", "Gasolina", "Oil Change", 16, models[0]),
                new Car("Pickup", "Azul", "Pickup de gran capacidad", "Ford", 15000f, 8, 3, 80f, "3.5L", "Diesel", "Tire Rotation", 20, models[1]),
                new Car("Deportivo", "Rojo", "Coche deportivo de alta gama", "Ferrari", 20000f, 2, 1, 200f, "6.5L", "Gasolina", "Full Service", 22, models[2]),
                new Car("Compacto", "Negro", "Coche compacto para ciudad", "Peugot", 8000f, 12, 6, 40f, "1.2L", "Gasolina", "Brake Inspection", 15, models[3])
            };

            ApplicationUser user = new ApplicationUser("1","Elena", "Navarro Martinez","elena@navarro");

            var rental = new Rental("Madrid Center", DateTime.Today.AddDays(7), Rental.RentalPaymentMethodEnum.Visa, DateTime.Today, DateTime.Today.AddDays(1), new List<RentalItem>(),user);
            var rentalItems = new RentalItem(1,1,rental);
            rental.RentalItems.Add(rentalItems);

            _context.Add(user);
            _context.AddRange(models);
            _context.AddRange(cars);
            _context.Add(rental);
            _context.SaveChanges();

        }
        public static IEnumerable<object[]> TestCasesFor_GetCarsForRental_OK()
        {

            var carDTOs = new List<CarForRentalDTO>() {
                new CarForRentalDTO(1,"Citroen c15","Gasolina","Citroen",50f,"Blanco"),
                new CarForRentalDTO(2,"Ford F150","Diesel","Ford",80f,"Azul"),
                new CarForRentalDTO(3,"Ferrari laFerrari","Gasolina","Ferrari",200f,"Rojo"),
                new CarForRentalDTO(4,"Peugot 305","Gasolina","Peugot",40f,"Negro")
            };

            var carDTOsTC1 = new List<CarForRentalDTO>() { carDTOs[0], carDTOs[1], carDTOs[2], carDTOs[3] };
            var carDTOsTC2 = new List<CarForRentalDTO>() { carDTOs[2] };
            var carDTOsTC3 = new List<CarForRentalDTO>() { carDTOs[0], carDTOs[3] };
            var carDTOsTC4 = new List<CarForRentalDTO>() { carDTOs[1] };

            var allTests = new List<object[]>
            {             
                new object[] { null, null, carDTOsTC1,  },
                new object[] { "Ferrari laFerrari", null,  carDTOsTC2, },
                new object[] { null, 80f, carDTOsTC3, },
                new object[] { "Ford F150", 100f, carDTOsTC4, },
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetCarsForRental_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCarsForRenting_OK_test(string? filtroModel, float? filtroRentingprice,
            IList<CarForRentalDTO> expectedCars)
        {
            // Arrange
            var mock = new Mock<ILogger<CarsController>>();
            ILogger<CarsController> logger = mock.Object;
            var controller = new CarsController(_context, logger);

            // Act
            var result = await controller.GetCars_ForRenting(filtroModel,filtroRentingprice);

            //Assert
            //we check that the response type is OK 
            var okResult = Assert.IsType<OkObjectResult>(result);
            //and obtain the list of cars
            var carDTOsActual = Assert.IsType<List<CarForRentalDTO>>(okResult.Value);
            Assert.Equal(expectedCars, carDTOsActual);

        }
    }
}
