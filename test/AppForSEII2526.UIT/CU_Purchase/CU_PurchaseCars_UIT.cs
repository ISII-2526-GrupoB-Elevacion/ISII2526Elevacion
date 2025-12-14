using AppForSEII2526.UIT.Shared;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Purchase
{
    public class CU_PurchaseCars_UIT : UC_UIT
    {
        private const int carId1 = 2;
        private const string model1 = "Toyota Corolla";
        private const string color1 = "Rojo";
        private const string fuelType1 = "Diesel";
        private const string manufacturer1 = "Toyota";
        private const string purchasingPrice1 = "5000 €";
        private const string description1 = "Automovil de turismo del segmento C";

        private const int carId2 = 1;
        private const string model2 = "Audi A4";
        private const string color2 = "Gris";
        private const string fuelType2 = "Gasolina";
        private const string manufacturer2 = "Audi";
        private const string purchasingPrice2 = "18000 €";
        private const string description2 = "Automovil de turismo del segmento D";

        private const string name = "Elena";
        private const string surname = "Navarro Martínez";
        private const string deliveryCarDealer = "C/Albacete nº 12";
        private const string quantity = "1";

        private const string paymentMethod1 = "Google Pay";
        private const string paymentMethod2 = "PayPal";

        private SelectCarsForPurchase_PO selectCarsForPurchase_PO;
        private CreatePurchase_PO createPurchase_PO;

        public CU_PurchaseCars_UIT(ITestOutputHelper output) : base(output)
        {
            selectCarsForPurchase_PO = new SelectCarsForPurchase_PO(_driver, _output);
            createPurchase_PO = new CreatePurchase_PO(_driver, _output);
        }

        private void Precondition_perform_login()
        {
            Perform_login("elena@uclm.es", "Password1234%");
        }

        /*
         * Gran parte de la función InitialStepsForPurchaseCars() está sacado de ChatGPT
         * debido a que, con el código del repositorio de ejemplo,
         * cuando hace el login algunas veces se quedaba pillado intentando hacer click en el botón de Purchase
         * para acceder a la sección del select, por algún tipo de click falso o precarga rara
         */
        private void InitialStepsForPurchaseCars()
        {
            Precondition_perform_login();
            Thread.Sleep(1000);
            selectCarsForPurchase_PO.WaitForBeingVisible(By.Id("CreatePurchase"));
            _driver.FindElement(By.Id("CreatePurchase")).Click();
        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_FA0_CU1_3_CarsNotAvailable()
        {
            //Arrange
            InitialStepsForPurchaseCars();

            //Act
            selectCarsForPurchase_PO.SearchCars("", "");

            //Assert
            var expectedMessage = "There are no cars available for being purchased.";
            Assert.True(selectCarsForPurchase_PO.CheckMessageErrorNotAvailableCars(expectedMessage));
        }

        [Theory]
        [InlineData(model1, color1, fuelType1, manufacturer1, purchasingPrice1, "Rojo", "")]
        [InlineData(model2, color2, fuelType2, manufacturer2, purchasingPrice2, "", "Audi A4")]
        [InlineData(model2, color2, fuelType2, manufacturer2, purchasingPrice2, "Gris", "Audi A4")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_FA1_CU1_4_5_6_Filtering(string model, string color, string fuelType, string manufacturer, string purchasingPrice, string filterColor, string filterModel)
        {
            //Arrange
            InitialStepsForPurchaseCars();
            var expectedCars = new List<string[]> { new string[] { model, color, fuelType, manufacturer, purchasingPrice }, };

            //Act
            selectCarsForPurchase_PO.SearchCars(filterColor, filterModel);

            //Assert
            Assert.True(selectCarsForPurchase_PO.CheckListOfCars(expectedCars));
        }

        [Theory]
        [InlineData(model1, model2, color1, color2, fuelType1, fuelType2, manufacturer1, manufacturer2, purchasingPrice1, purchasingPrice2, "", "")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_FA1_CU1_7_Filtering(string model1, string model2, string color1, string color2, string fuelType1, string fuelType2, string manufacturer1, string manufacturer2, string purchasingPrice1, string purchasingPrice2, string filterColor, string filterModel)
        {
            //Arrange
            InitialStepsForPurchaseCars();
            var expectedCars = new List<string[]> { new string[] { model2, color2, fuelType2, manufacturer2, purchasingPrice2 }, new string[] { model1, color1, fuelType1, manufacturer1, purchasingPrice1 } };

            //Act
            selectCarsForPurchase_PO.SearchCars(filterColor, filterModel);

            //Assert
            Assert.True(selectCarsForPurchase_PO.CheckListOfCars(expectedCars));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_FA2_CU1_8_PurchaseNotAvailable()
        {
            //Arrange
            InitialStepsForPurchaseCars();

            //Act
            selectCarsForPurchase_PO.AddCarToShoppingCart(model1);
            selectCarsForPurchase_PO.RemoveCarFromShoppingCart(model1);

            //Assert
            Assert.True(selectCarsForPurchase_PO.PurchasingNotAvailable());
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_FA3_CU1_9_DeleteSelectedCars()
        {
            //Arrange
            InitialStepsForPurchaseCars();

            //Act
            selectCarsForPurchase_PO.AddCarToShoppingCart(model1);
            selectCarsForPurchase_PO.AddCarToShoppingCart(model2);
            selectCarsForPurchase_PO.PurchaseCars();

            createPurchase_PO.FillInPurchaseQuantity(quantity, model1);
            createPurchase_PO.FillInPurchaseQuantity(quantity, model2);
            createPurchase_PO.PressModifyCars();

            selectCarsForPurchase_PO.RemoveCarFromShoppingCart(model2);
            selectCarsForPurchase_PO.PurchaseCars();

            //Assert
            var expectedTotalPrice = "5000";
            Assert.True(createPurchase_PO.CheckTotalPrice(expectedTotalPrice));
        }

        [Theory]
        [InlineData(model1, "", surname, deliveryCarDealer, paymentMethod1, quantity, "The field Name must be a string with a minimum length of 2 and a maximum length of 20.")]
        [InlineData(model1, name, "", deliveryCarDealer, paymentMethod1, quantity, "The field Surname must be a string with a minimum length of 4 and a maximum length of 100.")]
        [InlineData(model1, name, surname, "", paymentMethod1, quantity, "The field DeliveryCarDealer must be a string with a minimum length of 1 and a maximum length of 20.")]
        [InlineData(model1, name, surname, deliveryCarDealer, paymentMethod1, "", "The Quantity field must be a number.")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_FA4_CU1_10_11_12_13_Testing_Errors_Mandatory_Data(string model, string name, string surname, string deliveryCarDealer, string paymentMethod, string quantity, string expectedMessageError)
        {
            //Arrange
            InitialStepsForPurchaseCars();

            //Act
            selectCarsForPurchase_PO.AddCarToShoppingCart(model);
            selectCarsForPurchase_PO.PurchaseCars();

            createPurchase_PO.FillInPurchaseInfo(name, surname, deliveryCarDealer, paymentMethod);
            createPurchase_PO.FillInPurchaseQuantity(quantity, model);

            createPurchase_PO.PressPurchaseYourCars();

            //Assert
            Assert.True(createPurchase_PO.CheckValidationError(expectedMessageError), $"Expected error: {expectedMessageError}");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_FA5_CU1_15_ModifySelectedCars()
        {
            //Arrange
            InitialStepsForPurchaseCars();

            //Act
            selectCarsForPurchase_PO.AddCarToShoppingCart(model1);
            selectCarsForPurchase_PO.AddCarToShoppingCart(model2);
            selectCarsForPurchase_PO.PurchaseCars();

            createPurchase_PO.FillInPurchaseInfo(name, surname, deliveryCarDealer, paymentMethod1);
            createPurchase_PO.FillInPurchaseQuantity(quantity, model1);
            createPurchase_PO.FillInPurchaseQuantity(quantity, model2);

            createPurchase_PO.PressModifyCars();
            selectCarsForPurchase_PO.RemoveCarFromShoppingCart(model2);
            selectCarsForPurchase_PO.PurchaseCars();

            //Assert
            var expectedPurchaseItems = new List<string[]> { new string[] { model1, color1, description1, purchasingPrice1.Trim(' ', '€') }, };
            Assert.True(createPurchase_PO.CheckListOfPurchaseItems(expectedPurchaseItems, quantity, model1));
        }
    }
}
