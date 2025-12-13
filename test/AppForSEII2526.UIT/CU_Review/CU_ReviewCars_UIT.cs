using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.CU_Review;
using AppForSEII2526.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Review
{
    public class CU_ReviewCars_UIT : UC_UIT{

        private SelectCarsForReview_PO selectCarsForReview_PO;
        private const int carId1 = 1;
        private const string carModel1 = "Audi A4";
        private const string carCarClass1 = "Berlina";
        private const string carManufacturer1 = "Audi";
        private const string carFuelType1 = "Gasolina";
        private const string carColor1 = "Gris";

        private const string carModel2 = "Toyota Corolla";
        private const string carCarClass2 = "Familiar";
        private const string carManufacturer2 = "Toyota";
        private const string carFuelType2 = "Diesel";
        private const string carColor2 = "Rojo";

        public CU_ReviewCars_UIT(ITestOutputHelper output) : base(output) {

            selectCarsForReview_PO = new SelectCarsForReview_PO (_driver, _output);
        }
            
        private void Precondition_perform_login() {
            Perform_login("elena@uclm.es", "Password1234%");
        }

        private void InitialStepsForReviewCars()
        {
            Precondition_perform_login();

            // Espera de seguridad tras login
            Thread.Sleep(1000);

            var byCreate = By.Id("CreateReview");
            selectCarsForReview_PO.WaitForBeingClickable(byCreate);
            var element = _driver.FindElement(byCreate);


            IJavaScriptExecutor executor = (IJavaScriptExecutor)_driver;
            executor.ExecuteScript("arguments[0].click();", element);

            try
            {
                var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                wait.Until(d => d.Url.Contains("review"));
            }
            catch (WebDriverTimeoutException)
            {
                _driver.Navigate().GoToUrl(new Uri(_driver.Url).GetLeftPart(UriPartial.Authority) + "/review/select");
            }
        }

        [Theory]
        [InlineData(carModel1, carCarClass1, carManufacturer1, carFuelType1,carColor1, "Audi", "")]
        [InlineData(carModel2, carCarClass2, carManufacturer2, carFuelType2, carColor2, "", "Diesel")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_AF0_UC4_2_3_filtering(string carModel, string carCarClass,string carManufacturer, string carFuelType, string carColor, string filterManufacturer, string filterFuelType)
        {
            //Arrange
            InitialStepsForReviewCars();
            var expectedCars = new List<string[]> {
                new string[] { carModel, carCarClass, carManufacturer, carColor, carFuelType }
            };

            //Act
            selectCarsForReview_PO.SearchCars(filterManufacturer, filterFuelType);

            //Assert

            Assert.True(selectCarsForReview_PO.CheckListOfCars(expectedCars));

        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]

        public void UC4_AF1_UC4_4_ReviewNotavailable()
        {
            //Arrange
            InitialStepsForReviewCars();
            //Act
            selectCarsForReview_PO.AddCarToReviewCart(carModel1);
            selectCarsForReview_PO.RemoveCarFromReviewCart(carModel1);

            //Assert

            Assert.True(selectCarsForReview_PO.ReviewNotAvailable());

        }

    }

}

    
    
