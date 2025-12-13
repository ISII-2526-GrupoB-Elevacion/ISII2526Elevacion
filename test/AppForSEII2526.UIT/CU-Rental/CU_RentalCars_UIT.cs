using AppForSEII2526.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppForSEII2526.UIT.CU_RentalCars
{
    public class CU_RentalCars_UIT: UC_UIT
    {
        private SelectCarsForRental_PO selectCarsForRental_PO;

        private const int carId1 = 1;
        private const string CarModel1= "Audi A4";
        private const string FuelType1 = "Gasolina";
        private const string Manufacturer1= "Audi";
        private const string RentingPrice1 = "85";
        private const string Color1 = "Gris";


        private const string CarModel2 = "Toyota Corolla";
        private const string FuelType2 = "Diesel";
        private const string Manufacturer2 = "Toyota";
        private const string RentingPrice2 = "60";
        private const string Color2 = "Rojo";
        public CU_RentalCars_UIT(ITestOutputHelper output): base(output)
        {
            selectCarsForRental_PO = new SelectCarsForRental_PO(_driver, _output);
        }
        private void Precondition_perform_login()
        {
            Perform_login("elena@uclm.es", "Password1234%");
        }
        private void InitialStepsForRentalCars()
        {
            Precondition_perform_login();

            // Espera de seguridad tras login
            Thread.Sleep(1000);

            var byCreate = By.Id("CreateRental");
            selectCarsForRental_PO.WaitForBeingClickable(byCreate);
            var element = _driver.FindElement(byCreate);


            IJavaScriptExecutor executor = (IJavaScriptExecutor)_driver;
            executor.ExecuteScript("arguments[0].click();", element);

            try
            {
                var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                wait.Until(d => d.Url.Contains("rental"));
            }
            catch (WebDriverTimeoutException)
            {
                _driver.Navigate().GoToUrl(new Uri(_driver.Url).GetLeftPart(UriPartial.Authority) + "/rental/select");
            }


        }
        [Theory]
        [InlineData(CarModel1, FuelType1, Manufacturer1, RentingPrice1, Color1, "Audi A4", null)]
        [InlineData(CarModel2, FuelType2, Manufacturer2, RentingPrice2, Color2, "", 60f)]
        [InlineData(CarModel2, FuelType2, Manufacturer2, RentingPrice2, Color2, "Toyota Corolla", 60f)]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU2_FA1_CU2_3_4_5_filtering(string carmodel, string fueltype, string manufacturer, string rentingprice, string color ,string filterCarModel, float ? filterRentingPrice)
        {
            //Arrange
            InitialStepsForRentalCars();
            var expectedCars = new List<string[]> { new string[] { carmodel, fueltype, manufacturer, rentingprice, color }, };

            //Act
            selectCarsForRental_PO.SearchCars(filterCarModel,filterRentingPrice,"","","");
            Thread.Sleep(1000);
            //Assert
            Assert.True(selectCarsForRental_PO.CheckListOfCars(expectedCars));

        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]

        public void CU2_AF2_UC2_6_RentingNotavailable()
        {
            //Arrange
            InitialStepsForRentalCars();
            //Act
            selectCarsForRental_PO.AddCarToRentingCart(CarModel1);
            selectCarsForRental_PO.RemoveCarFromRentingCart(CarModel1);

            //Assert

            Assert.True(selectCarsForRental_PO.RentingNotAvailable());

        }
    }
}
