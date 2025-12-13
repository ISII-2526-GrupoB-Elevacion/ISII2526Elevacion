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
        private const int carId1 = 1;
        private const string model1 = "Toyota Corolla";
        private const string color1 = "Rojo";
        private const string fuelType1 = "Diesel";
        private const string manufacturer1 = "Toyota";
        private const string purchasingPrice1 = "5000 €";

        private const int carId2 = 2;
        private const string model2 = "Audi A4";
        private const string color2 = "Gris";
        private const string fuelType2 = "Gasolina";
        private const string manufacturer2 = "Audi";
        private const string purchasingPrice2 = "18000 €";

        private SelectCarsForPurchase_PO selectCarsForPurchase_PO;

        public CU_PurchaseCars_UIT(ITestOutputHelper output) : base(output)
        {
            selectCarsForPurchase_PO = new SelectCarsForPurchase_PO(_driver, _output);
        }

        private void Precondition_perform_login()
        {
            Perform_login("elena@uclm.es", "Password1234%");
        }

        private void InitialStepsForPurchaseCars()
        {
            Precondition_perform_login();
            var byCreate = By.Id("CreatePurchase");
            const int maxAttempts = 10;
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    selectCarsForPurchase_PO.WaitForBeingClickable(byCreate);
                    _driver.FindElement(byCreate).Click();
                    break;
                }
                catch (StaleElementReferenceException)
                {
                    if (attempt == maxAttempts) throw;
                    Thread.Sleep(250);
                }
            }
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
        public void CU1_FA1_CU1_8_PurchaseNotAvailable()
        {
            //Arrange
            InitialStepsForPurchaseCars();

            //Act
            selectCarsForPurchase_PO.AddCarToShoppingCart(model1);
            selectCarsForPurchase_PO.RemoveCarFromShoppingCart(model1);

            //Assert
            Assert.True(selectCarsForPurchase_PO.PurchasingNotAvailable());
        }
    }
}
