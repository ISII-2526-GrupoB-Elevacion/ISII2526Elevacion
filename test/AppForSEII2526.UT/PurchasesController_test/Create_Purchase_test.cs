using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PurchaseDTO;
using AppForSEII2526.API.Models;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.PurchasesController_test
{
    public class Create_Purchase_test: AppForSEII25264SqliteUT
    {
        private const string UserName = "elena@navarro";
        private const string Name = "Elena";
        private const string Surname = "Navarro Martinez";
        private const string DeliveryCarDealer = "Madrid Center";

        private const string car1Model = "Audi A4";
        private const string car2Model = "Citroen C15";

        public Create_Purchase_test()
        {
            var models = new List<Model> //me creo modelos de coche de prueba
            {
                new Model(car1Model),
                new Model(car2Model)
            };

            var cars = new List<Car> //me creo filas de una tabla coche de prueba
            {
                new Car("Sedan", "Negro", "Auto elegante y cómodo ideal para viajes largos.", "Audi", 18000f, 1, 3, 120f, "2.0L", "Gasolina", "Anual", 45, models[0]),
                new Car("Furgoneta", "Blanco", "Vehiculo espacioso y resistente para transporte de carga.", "Citroen", 12000f, 2, 1, 80f, "1.8L", "Diesel", "Semestral", 40, models[1]),
            };

            ApplicationUser user = new ApplicationUser("1", Name, Surname, UserName); //me creo un usuario ficticio

            var purchase = new Purchase(DeliveryCarDealer, Purchase.PurchasePaymentMethodEnum.Visa, DateTime.Today, new List<PurchaseItem>(), user); //me creo una compra ficticia
            var purchaseItems = new PurchaseItem(1, 1, purchase);
            purchase.PurchaseItems.Add(purchaseItems);

            _context.Add(user); //lo añado todo
            _context.AddRange(models);
            _context.AddRange(cars);
            _context.Add(purchase);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreatePurchases() //prueba para el error
        {
            var purchaseNoItem = new PurchaseForCreateDTO(0f, Name, Surname, UserName, DeliveryCarDealer, Purchase.PurchasePaymentMethodEnum.Visa, new List<PurchaseItemDTO>()); //le paso dto sin coches para comprar

            var purchaseItems = new List<PurchaseItemDTO>() { new PurchaseItemDTO(car2Model, 1, 0f, "Blanco") };

            var purchaseApplicationUser = new PurchaseForCreateDTO(0f, Name, Surname, "victor@lopez", DeliveryCarDealer, Purchase.PurchasePaymentMethodEnum.Visa, purchaseItems); //le paso dto con usuario que no existe

            var purchaseCarNotExists = new PurchaseForCreateDTO(0f, Name, Surname, UserName, DeliveryCarDealer, Purchase.PurchasePaymentMethodEnum.Visa, new List<PurchaseItemDTO>() { new PurchaseItemDTO("Ferrari La Ferrari", 1, 0f, "Rojo") }); //le paso un dto para un coche que no existe

            var purchaseCarNotAvailable = new PurchaseForCreateDTO(0f, Name, Surname, UserName, DeliveryCarDealer, Purchase.PurchasePaymentMethodEnum.Visa, new List<PurchaseItemDTO>() { new PurchaseItemDTO(car1Model, 1, 0f, "Negro") }); //le paso un dto para un coche del que no quedan unidades para comprar

            var purchaseTooManyCars = new PurchaseForCreateDTO(0f, Name, Surname, UserName, DeliveryCarDealer, Purchase.PurchasePaymentMethodEnum.Visa, new List<PurchaseItemDTO> { new PurchaseItemDTO(car2Model, 0f, "Blanco", 2, "") });

            var allTests = new List<object[]> //errores esperados para cada entrada de las anteriores, PUESTOS EN ORDEN
            {             //input for create purchase- Error expected
                
                new object[] { purchaseNoItem, "Error! You must include at least one car to be purchased", },
                new object[] { purchaseApplicationUser, "Error! UserName is not registered", },
                new object[] { purchaseCarNotExists, "Error! The car Ferrari La Ferrari is not for sale at the dealership", },
                new object[] { purchaseCarNotAvailable, "Error! There are not enough units available to purchase the car Audi A4", },
                new object[] { purchaseTooManyCars, "Error! You're buying too many cars without description", },
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreatePurchases))]
        public async Task CreatePurchase_Error_test(PurchaseForCreateDTO purchaseDTO, string errorExpected) //prueba para el errors, le pasa los casos de prueba anteriormente preparados
        {
            // Arrange
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;

            var controller = new PurchasesController(_context, logger);

            // Act
            var result = await controller.CreatePurchase(purchaseDTO); //ejecuto el método

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            //we check that the expected error message and actual are the same
            Assert.StartsWith(errorExpected, errorActual); //compruebo si el error que sale es mi error esperado
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreatePurchase_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;

            var controller = new PurchasesController(_context, logger);

            var purchaseDTO = new PurchaseForCreateDTO(0f, Name, Surname, UserName, DeliveryCarDealer, Purchase.PurchasePaymentMethodEnum.Visa, new List<PurchaseItemDTO>() { new PurchaseItemDTO(car2Model, 1, 0f, "Blanco") }); //creo una compra valida

            var expectedpurchaseDetailDTO = new PurchaseDetailDTO(2, Name, Surname, UserName, DeliveryCarDealer, DateTime.Today, 12000f, new List<PurchaseItemDTO> { new PurchaseItemDTO(car2Model, 1, 12000f, "Blanco")}); //lo que espero que se añada a la base de datos a partir de la compra válida (es decir lo que se le muestra al usuario que es el details)

            // Act
            var result = await controller.CreatePurchase(purchaseDTO); //ejecuto el método

            //Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualPurchaseDetailDTO = Assert.IsType<PurchaseDetailDTO>(createdResult.Value); //obtendo la salida que me da el programa

            Assert.Equal(expectedpurchaseDetailDTO, actualPurchaseDetailDTO); //compruebo el details que me saca con respecto a lo que yo espero

        }
    }
}
