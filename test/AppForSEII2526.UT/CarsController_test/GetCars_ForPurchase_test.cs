using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.CarDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.CarsController_test
{
    public class GetCars_ForPurchase_test : AppForSEII25264SqliteUT
    {
        public GetCars_ForPurchase_test()
        {
            var models = new List<Model>() //me creo modelos de coche de prueba
            {
                new Model ("Audi A4"),
                new Model ("Citroen C15"),
                new Model ("Seat Leon"),
                new Model ("Peugeot 308")
            };

            var cars = new List<Car> //me creo filas de una tabla coche de prueba
            {
                new Car("Sedan", "Negro", "Auto elegante y cómodo ideal para viajes largos.", "Audi", 18000f, 5, 3, 120f, "2.0L", "Gasolina", "Anual", 45, models[0]),
                new Car("Furgoneta", "Blanco", "Vehiculo espacioso y resistente para transporte de carga.", "Citroen", 12000f, 2, 1, 80f, "1.8L", "Diesel", "Semestral", 40, models[1]),
                new Car("Compacto", "Rojo", "Auto urbano agil con excelente eficiencia de combustible.", "Seat", 15000f, 4, 2, 90f, "1.6L", "Gasolina", "Anual", 42, models[2]),
                new Car("Hatchback", "Rojo", "Coche moderno con gran rendimiento y confort interior.", "Peugeot", 17000f, 6, 3, 100f, "1.5L", "Diesel", "Trimestral", 44, models[3])
            };

            ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martinez", "elena@uclm.es"); //me creo un usuario ficticio

            var purchase = new Purchase("C/Prim nº 7", Purchase.PurchasePaymentMethodEnum.Visa, DateTime.Today, new List<PurchaseItem>(), user); //me creo una compra ficticia
            var purchaseItems = new PurchaseItem(1, 1, purchase);
            purchase.PurchaseItems.Add(purchaseItems);

            _context.Add(user); //lo añado todo
            _context.AddRange(models);
            _context.AddRange(cars);
            _context.Add(purchase);
            _context.Add(purchaseItems);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetCarsForPurchase_OK()
        {
            var carDTOs = new List<CarForPurchaseDTO>() { //me creo los propios dtos que me va a sacar el método select
                new CarForPurchaseDTO(1,"Audi A4","Negro","Gasolina","Audi",18000f),
                new CarForPurchaseDTO(2,"Citroen C15","Blanco","Diesel","Citroen",12000f),
                new CarForPurchaseDTO(3,"Seat Leon","Rojo","Gasolina","Seat",15000f),
                new CarForPurchaseDTO(4,"Peugeot 308","Rojo","Diesel","Peugeot",17000f),
            };

            var carDTOsTC1 = new List<CarForPurchaseDTO>() { carDTOs[0], carDTOs[1], carDTOs[2], carDTOs[3] }; //según las entradas que tengo pensado meterle me tiene que salir esto
            var carDTOsTC2 = new List<CarForPurchaseDTO>() { carDTOs[2], carDTOs[3] };
            var carDTOsTC3 = new List<CarForPurchaseDTO>() { carDTOs[3] };
            var carDTOsTC4 = new List<CarForPurchaseDTO>() { carDTOs[0] };

            var allTests = new List<object[]>
            {
                new object[] { null, null, carDTOsTC1 }, //preparo los propios casos de prueba (entradas + salidas esperadas)
                new object[] { "Rojo", null, carDTOsTC2 },
                new object[] { null, "Peugeot", carDTOsTC3 },
                new object[] { "Negro", "Audi", carDTOsTC4 },
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetCarsForPurchase_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCarsForPurchase_OK_test(string? filtroColor, string? modelo, IList<CarForPurchaseDTO> expectedCars) //le paso los parámetros más los propios tests
        {
            // Arrange
            var mock = new Mock<ILogger<CarsController>>();
            ILogger<CarsController> logger = mock.Object;
            var controller = new CarsController(_context, logger);
            // Act
            var result = await controller.GetCars_ForPurchase(filtroColor, modelo); //ejecuto el método select con todas las entradas preparadas
            //Assert
            //we check that the response type is OK 
            var okResult = Assert.IsType<OkObjectResult>(result);
            //and obtain the list of cars
            var carDTOsActual = Assert.IsType<List<CarForPurchaseDTO>>(okResult.Value); //obtengo la salida
            Assert.Equal(expectedCars, carDTOsActual); //compra si ambos resultados son iguales, si es así devuelve ok para la prueba, si es que no devuelve error
        }
    }
}
