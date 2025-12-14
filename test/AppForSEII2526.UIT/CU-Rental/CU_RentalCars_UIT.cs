using AppForSEII2526.UIT.CU_Purchase;
using AppForSEII2526.UIT.CU_Rental;
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
        private CreateRental_PO createRental_PO;

        private const string Name = "Elena";
        private const string SurName = "Navarro Martínez";
        private const string DeliveryCarDealer = "Calle Albacete";
        private const string PaymentMethod = "Visa";
        private const string Quantity= "1";

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
            createRental_PO = new CreateRental_PO(_driver, _output);
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
        public void CU2_FA2_CU2_6_RentingNotavailable()
        {
            //Arrange
            InitialStepsForRentalCars();
            //Act
            selectCarsForRental_PO.AddCarToRentingCart(CarModel1);
            selectCarsForRental_PO.RemoveCarFromRentingCart(CarModel1);

            //Assert

            Assert.True(selectCarsForRental_PO.RentingNotAvailable());

        }
        public static IEnumerable<object[]> TestCasesFor_CU2_9_10_FA4_errorindates()
        {
            var allTests = new List<object[]> {
                new object[] {DateTime.Today.AddDays(-1), DateTime.Today.AddDays(2), DateTime.Today.AddDays(1), "Your rental period must be later",  },
                //cannot be checked if datetime is before today, because the next condition is checked before
                new object[] {DateTime.Today.AddDays(2), DateTime.Today.AddDays(1), DateTime.Today.AddDays(1), "Your rental must end after than its starts", },
            };

            return allTests;
        }

        public void CU2_FA3_CU1_7_DeleteSelectedCars()
        {
            //Arrange
            InitialStepsForRentalCars();

            //Act
            selectCarsForRental_PO.AddCarToRentingCart(CarModel1);
            selectCarsForRental_PO.AddCarToRentingCart(CarModel2);
            selectCarsForRental_PO.RentCars();

            createRental_PO.FillInPurchaseQuantity(Quantity, CarModel1);
            createRental_PO.FillInPurchaseQuantity(Quantity, CarModel2);
            createRental_PO.PressModifyCars();

            selectCarsForRental_PO.RemoveCarFromRentingCart(CarModel2);
            selectCarsForRental_PO.RentCars();

            //Assert
            var expectedTotalPrice = "85";
            Assert.True(createRental_PO.CheckTotalPrice(expectedTotalPrice));
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_CU2_9_10_FA4_errorindates))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU2_8_9_FA2_errorindates(DateTime from, DateTime to, DateTime renting, string expectedMessageError)
        {
            //Arrange


            //Act
            InitialStepsForRentalCars();

            selectCarsForRental_PO.SearchCars("", null, from.ToString("dd-MM-yyyy"), to.ToString("dd-MM-yyyy"), renting.ToString("dd-MM-yyyy"));
            Thread.Sleep(1000);
            //Assert

            //this message will be shown if assert fails
            Assert.True(selectCarsForRental_PO.CheckMessageError(expectedMessageError), $"Expected error: {expectedMessageError}");
        }

        [Theory]
        [InlineData("",SurName,DeliveryCarDealer,PaymentMethod,"The field Name must be a string with a minimum length of 2 and a maximum length of 20.")]
        [InlineData(Name, "", DeliveryCarDealer, PaymentMethod, "The field Surname must be a string with a minimum length of 4 and a maximum length of 100.")]
        [InlineData(Name, SurName, "", PaymentMethod, "The field DeliveryCarDealer must be a string with a minimum length of 1 and a maximum length of 20.")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU2_10_11_12_FA4_testingErrorsMandatorydata(string name, string surname, string deliveryCarDealer,string paymentMethod,string expectedMessageError)
        {
            //Arrange

            var from = DateTime.Today.AddDays(1);
            var to = DateTime.Today.AddDays(2);
            var renting = DateTime.Today.AddDays(1);
            //Act
            InitialStepsForRentalCars();

            selectCarsForRental_PO.SearchCars("",null,from.ToString("dd-MM-yyyy"), to.ToString("dd-MM-yyyy"), renting.ToString("dd-MM-yyyy"));
            selectCarsForRental_PO.AddCarToRentingCart(CarModel1);
            selectCarsForRental_PO.RentCars();
            createRental_PO.FillInRentalInfo(name,surname,deliveryCarDealer,paymentMethod);
            createRental_PO.PressRentYourCars();

            //Assert
            //the expected error is shown in the view
            Assert.True(createRental_PO.CheckValidationError(expectedMessageError), $"Expected error: {expectedMessageError}");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU2_13_FA5_ModifyRentalItems()
        {
            //Arrange

            var from = DateTime.Today.AddDays(1);
            var to = DateTime.Today.AddDays(2);
            var renting = DateTime.Today.AddDays(1);
            //Act
            InitialStepsForRentalCars();

            selectCarsForRental_PO.SearchCars("", null, from.ToString("dd-MM-yyyy"), to.ToString("dd-MM-yyyy"), renting.ToString("dd-MM-yyyy"));
            selectCarsForRental_PO.AddCarToRentingCart(CarModel1);
            selectCarsForRental_PO.AddCarToRentingCart(CarModel2);
            selectCarsForRental_PO.RentCars();
            createRental_PO.PressModifyCars();
            //we remove movietitle2 from the rentingcart
            selectCarsForRental_PO.RemoveCarFromRentingCart(CarModel1);
            selectCarsForRental_PO.RentCars();

            //Assert
            //the list of cars must change
            var expectedRentalItems = new List<string[]> { new string[] { CarModel2, Manufacturer2, RentingPrice2 }, };
            Assert.True(createRental_PO.CheckListOfRentalItems(expectedRentalItems));
        }
    }
}
