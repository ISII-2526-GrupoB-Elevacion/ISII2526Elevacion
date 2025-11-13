using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PurchaseDTO;
using AppForSEII2526.API.DTOs.RentalDTO;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.PurchasesController_test
{
    public class Get_Details_Purchase_test: AppForSEII25264SqliteUT
    {
        public Get_Details_Purchase_test()
        {
            var models = new List<Model>() //me creo modelos de coche de prueba
            {
                new Model ("Audi A4"),
                new Model ("Citroen C15"),
            };

            var cars = new List<Car> //me creo filas de una tabla coche de prueba
            {
                new Car("Sedan", "Negro", "Auto elegante y cómodo ideal para viajes largos.", "Audi", 18000f, 5, 3, 120f, "2.0L", "Gasolina", "Anual", 45, models[0]),
                new Car("Furgoneta", "Blanco", "Vehiculo espacioso y resistente para transporte de carga.", "Citroen", 12000f, 2, 1, 80f, "1.8L", "Diesel", "Semestral", 40, models[1]),
            };

            ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martinez", "elena@uclm.es"); //me creo un usuario ficticio

            var purchase = new Purchase("C/Prim nº 7", Purchase.PurchasePaymentMethodEnum.Visa, DateTime.Today, new List<PurchaseItem>(), user); //me creo una compra ficticia
            var purchaseItems = new PurchaseItem(1, 1, purchase);
            purchase.PurchaseItems.Add(purchaseItems);
            foreach(var item in purchase.PurchaseItems)
            {
                purchase.PurchasingPrice += item.Quantity * cars[item.CarId-1].PurchasingPrice; //calculo el precio total de la compra
            }

            _context.Add(user); //lo añado todo a mi base de datos ficticia
            _context.AddRange(models);
            _context.AddRange(cars);
            _context.Add(purchase);
            _context.Add(purchaseItems);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDetailsPurchase_NotFound_test()
        {
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;

            var controller = new PurchasesController(_context, logger);

            // Act
            var result = await controller.Get_Details_Purchase(0);

            //Assert
            Assert.IsType<NotFoundResult>(result); //le paso un id de una compra que no existe y compruebo que es de tipo la salida NotFound (la que se esperaría)
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetDetailsPurchase_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;
            var controller = new PurchasesController(_context, logger);

            var expectedPurchase = new PurchaseDetailDTO("Elena", "Navarro Martinez", "C/Prim nº 7", DateTime.Today, 0, new List<PurchaseItemDTO>()); //creo un details que espero que me devuelva el programa, de una compra en concreto
            expectedPurchase.PurchaseItems.Add(new PurchaseItemDTO("Audi A4", 1, 18000f, "Negro"));
            foreach(var item in expectedPurchase.PurchaseItems)
            {
                expectedPurchase.PurchasingPrice += item.Quantity * item.PurchasingPrice; //calculo el precio esperado de compra
            }

            // Act 
            var result = await controller.Get_Details_Purchase(1); //llamo al método para que devuelva los detalles de una compra que de verdad esté en la base de datos

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var purchaseDTOactual = Assert.IsType<PurchaseDetailDTO>(okResult.Value);
            var eq = expectedPurchase.Equals(purchaseDTOactual);
            Assert.Equal(expectedPurchase, purchaseDTOactual); //compruebo que mi details coincide con lo que me saca el programa
        }
    }
}
