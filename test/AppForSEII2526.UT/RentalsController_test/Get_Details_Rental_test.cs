using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.RentalDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.RentalsController_test
{
    public class Get_Details_Rental_test : AppForSEII25264SqliteUT
    {
        public Get_Details_Rental_test()
        {
            var models = new List<Model> //creo una lista de modelos
            {
                new Model("Citroen c15"),
                new Model("Ford F150")
            };

            var cars = new List<Car> //creo una lista de coches de los respectivos modelos
            {
                new Car("Furgoneta", "Blanco", "La furgoneta", "Citroen", 5000f, 10, 5, 50f, "1.4L", "Gasolina", "Oil Change", 16, models[0]),
                new Car("Pickup", "Azul", "Pickup de gran capacidad", "Ford", 15000f, 8, 3, 80f, "3.5L", "Diesel", "Tire Rotation", 20, models[1])
            };

            ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martinez", "elena@navarro"); //creo un usuario ficticio

            var rental = new Rental("Madrid Center", DateTime.Today.AddDays(7), Rental.RentalPaymentMethodEnum.Visa, DateTime.Today.AddDays(1), DateTime.Today, new List<RentalItem>(), user); //creo un alquiler
            var rentalItems = new RentalItem(1, 1, rental);
            rental.RentalItems.Add(rentalItems);
            foreach (var item in rental.RentalItems)
            {
                rental.TotalPrice += (float)(cars[item.CarId-1].RentingPrice*item.Quantity*(rental.EndDate-rental.StartDate).TotalDays);
            }

            _context.Add(user); //añado todo a la base de datos
            _context.AddRange(models);
            _context.AddRange(cars);
            _context.Add(rental);
            _context.SaveChanges();

        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDetailsRental_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;

            var controller = new RentalsController(_context, logger);

            // Act
            var result = await controller.GetDetailsRental(0); //intento conseguir un alquiler inexistente

            //Assert
            Assert.IsType<NotFoundResult>(result); //compruebo si es correcto el resultado

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetDetailsRental_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;
            var controller = new RentalsController(_context, logger);

            //creo el dto esperado a la salida
            var expectedRental = new RentalDetailDTO("Elena", "Navarro Martinez", "Madrid Center", Rental.RentalPaymentMethodEnum.Visa, DateTime.Today, DateTime.Today.AddDays(7), DateTime.Today.AddDays(1), 0,new List<RentalItemDTO>());
            expectedRental.RentalItems.Add(new RentalItemDTO("Citroen c15", "Citroen", 50f,1));
            foreach (var item in expectedRental.RentalItems)
            {
                expectedRental.TotalPrice += (float)(item.RentingPrice * item.Quantity * (expectedRental.EndDate - expectedRental.StartDate).TotalDays);
            }

            // Act 
            var result = await controller.GetDetailsRental(1); //busco el alquiler esperado

            //Assert
            //we check that the response type is OK and obtain the rental
            var okResult = Assert.IsType<OkObjectResult>(result);
            var rentalDTOActual = Assert.IsType<RentalDetailDTO>(okResult.Value);
            var eq = expectedRental.Equals(rentalDTOActual); //compruebo si el alquiler esperado es igual al obtenido
            //we check that the expected and actual are the same
            Assert.Equal(expectedRental, rentalDTOActual);

        }
    }
}
