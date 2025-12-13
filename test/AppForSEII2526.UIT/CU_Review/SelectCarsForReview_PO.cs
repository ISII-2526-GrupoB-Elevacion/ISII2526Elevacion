using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.CU_Review
{
    public class SelectCarsForReview_PO : PageObject
    {
        By inputManufacturer = By.Id("inputManufacturer");
        By inputFuelType = By.Id("inputFuelType");
        By buttonSearchCars = By.Id("searchCars");
        By tableOfCarsBy = By.Id("TableOfCars");
        By errorShownBy = By.Id("ErrorsShown");
        By buttonReviewCars = By.Id("reviewCarButton");

        public SelectCarsForReview_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void SearchCars(string manufacturer, string fueltype)
        {
            //wait for the webelement to be clickable
            WaitForBeingClickable(inputManufacturer);
            _driver.FindElement(inputManufacturer).SendKeys(manufacturer);
            if (fueltype == "") fueltype = "All";
            SelectElement selectElement = new SelectElement(_driver.FindElement(inputFuelType));
            selectElement.SelectByText(fueltype);

            _driver.FindElement(buttonSearchCars).Click();


        }

        public bool CheckListOfCars(List<string[]> expectedCars)
        {

            return CheckBodyTable(expectedCars, tableOfCarsBy);
        }

        public bool CheckMessageError(string errorMessage)
        {
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"actual Message shown:{actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }

        public void AddCarToReviewCart(string carModel)
        {
            WaitForBeingClickable(By.Id("carToReview_" + carModel));

            _driver.FindElement(By.Id("carToReview_" + carModel)).Click();
        }

        public void RemoveCarFromReviewCart(string  carModel)
        {
            WaitForBeingClickable(By.Id("removeCar_" + carModel));
            _driver.FindElement(By.Id("removeCar_" + carModel)).Click();
        }

        public bool ReviewNotAvailable()
        {
            //the button is not Displayed=hidden

            return _driver.FindElement(buttonReviewCars).Displayed == false;
        }
    }
}
