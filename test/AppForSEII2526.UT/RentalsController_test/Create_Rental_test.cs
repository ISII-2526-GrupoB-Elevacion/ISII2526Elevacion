using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.RentalDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.RentalsController_test
{
    public class Create_Rental_test : AppForSEII25264SqliteUT
    {

        private const string UserName = "elena@navarro";
        private const string Name = "Elena";
        private const string Surname = "Navarro Martinez";
        private const string DeliveryCarDealer = "Calle Madrid";

        private const string car1Model = "Citroen c15";
        private const string car2Model = "Ford F150";


        public Create_Rental_test()
        {
            var models = new List<Model> //creo los distintos modelos
            {
                new Model(car1Model),
                new Model(car2Model)
            };

            var cars = new List<Car> //creo la lista de coches de los distintos modelos
            {
                new Car("Furgoneta", "Blanco", "La furgoneta", "Citroen", 5000f, 10, 1, 50f, "1.4L", "Gasolina", "Oil Change", 16, models[0]),
                new Car("Pickup", "Azul", "Pickup de gran capacidad", "Ford", 15000f, 8, 3, 80f, "3.5L", "Diesel", "Tire Rotation", 20, models[1])
            };

            ApplicationUser user = new ApplicationUser("1", Name, Surname, UserName); //creo un usuario ficticio

            var rental = new Rental(DeliveryCarDealer, DateTime.Today.AddDays(7), Rental.RentalPaymentMethodEnum.Visa, DateTime.Today.AddDays(1), DateTime.Today, new List<RentalItem>(), user); //creo un alquiler asociado al usuario
            var rentalItems = new RentalItem(1, 1, rental);
            rental.RentalItems.Add(rentalItems);

            _context.Add(user); //añado todo a la base de datos ficticia
            _context.AddRange(models);
            _context.AddRange(cars);
            _context.Add(rental);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreateRentals() //creo los distintos casos de prueba y los añado a una lista
        {
            var rentalNoITem = new RentalForCreateDTO(UserName, Name, Surname,
                DeliveryCarDealer, Rental.RentalPaymentMethodEnum.Visa,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(7), DateTime.Today.AddDays(3), 0f, new List<RentalItemDTO>());

            var rentalItems = new List<RentalItemDTO>() { new RentalItemDTO(car2Model,"Ford",0f,1) };

            var rentalFromBeforeToday = new RentalForCreateDTO(UserName, Name,Surname,
                DeliveryCarDealer,Rental.RentalPaymentMethodEnum.Visa,
                DateTime.Today, DateTime.Today.AddDays(5),DateTime.Today.AddDays(1),0f,rentalItems);

            var rentalToBeforeFrom = new RentalForCreateDTO(UserName,Name,Surname,
                DeliveryCarDealer, Rental.RentalPaymentMethodEnum.Visa,
                DateTime.Today.AddDays(5), DateTime.Today.AddDays(2),DateTime.Today.AddDays(1),0f, rentalItems);


            var RentalApplicationUser = new RentalForCreateDTO("victor@lopez", Name,Surname,
                DeliveryCarDealer, Rental.RentalPaymentMethodEnum.Visa,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(4),DateTime.Today.AddDays(3),0f, rentalItems);

            var BadDirection = new RentalForCreateDTO(UserName, Name, Surname,
                "Madrid Center", Rental.RentalPaymentMethodEnum.Visa,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(4), DateTime.Today.AddDays(3), 0f, rentalItems);

            var rentalCarNotAvailable = new RentalForCreateDTO(UserName, Name,Surname,
                DeliveryCarDealer, Rental.RentalPaymentMethodEnum.Visa,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),DateTime.Today.AddDays(3),0f,
                new List<RentalItemDTO>() { new RentalItemDTO(car1Model, "Citroen",0f,1) });


            var allTests = new List<object[]> //añado las respectivas pruebas a una lista con sus respectivos errores
            {             //input for create rental- Error expected
                
                new object[] { rentalNoITem, "Error! You must include at least one car to be rented",  },
                new object[] { rentalFromBeforeToday, "Error! Your rental date must start later than today", },
                new object[] { rentalToBeforeFrom, "Error! Your rental must end later than it starts", },
                new object[] { RentalApplicationUser, "Error! UserName is not registered", },
                new object [] { BadDirection, "Error! La dirección de envío debe empezar por la palabra Calle", },
                new object[] { rentalCarNotAvailable, "Error! Car 'Citroen c15' is not available for being rented from", },
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateRentals))]
        public async Task CreateRental_Error_test(RentalForCreateDTO rentalDTO, string errorExpected)
        {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;

            var controller = new RentalsController(_context, logger);

            // Act
            var result = await controller.CreateRental(rentalDTO); //intento crear el alquiler

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0]; //obtengo los datos del error

            //we check that the expected error message and actual are the same
            Assert.StartsWith(errorExpected, errorActual); //compruebo si el error saliente es igual al error esperado

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateRental_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;

            var controller = new RentalsController(_context, logger);

            DateTime from = DateTime.Today.AddDays(6);
            DateTime  to= DateTime.Today.AddDays(7);

            var rentalDTO = new RentalForCreateDTO(UserName, Name,Surname, //me creo el alquiler a probar
                DeliveryCarDealer, Rental.RentalPaymentMethodEnum.Visa,
                from, to, from, 0f, new List<RentalItemDTO>()
                { new RentalItemDTO(car2Model, "Ford", 0f, 1) });

            var expectedrentalDetailDTO = new RentalDetailDTO(Name,Surname,DeliveryCarDealer, //creo el detalle esperado con respecto al alquiler
                Rental.RentalPaymentMethodEnum.Visa,from,to,from,80f,
                new List<RentalItemDTO> { new RentalItemDTO(car2Model, "Ford", 80f, 1) });

            // Act
            var result = await controller.CreateRental(rentalDTO); //creo el alquiler

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualRentalDetailDTO = Assert.IsType<RentalDetailDTO>(createdResult.Value); //obtengo el detalle resultante

            Assert.Equal(expectedrentalDetailDTO, actualRentalDetailDTO); //compruebo si el detalle esperado es igual al detalle obtenido

        }
    }
}
