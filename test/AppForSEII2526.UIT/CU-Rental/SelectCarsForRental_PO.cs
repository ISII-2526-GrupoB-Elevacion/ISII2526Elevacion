using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;


namespace AppForSEII2526.UIT.CU_RentalCars
{
    internal class SelectCarsForRental_PO: PageObject
    {
        By ModelName = By.Id("modelname");
        By RentingPrice = By.Id("rentingprice");
        By buttonSearchCars= By.Id("searchCars");
        By inputFrom = By.Id("fromDate");
        By inputTo = By.Id("toDate");
        By inputRenting = By.Id("RentingDate");
        By tableOfCarsBy = By.Id("TableOfCars");
        By buttonRentCar = By.Id("rentCarButton");
        public SelectCarsForRental_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void SearchCars(string model, float ? rentingprice, string from, string to, string renting)
        {
            //wait for the webelement to be clickable
            WaitForBeingClickable(ModelName);
            _driver.FindElement(ModelName).SendKeys(model);
            _driver.FindElement(RentingPrice).SendKeys(rentingprice.ToString());

            if (from != "")
                _driver.FindElement(inputFrom).SendKeys(from);
            if (to != "")
                _driver.FindElement(inputTo).SendKeys(to);
            if (renting != "")
                _driver.FindElement(inputRenting).SendKeys(renting);

            _driver.FindElement(buttonSearchCars).Click();
        }

        public bool CheckListOfCars(List<string[]> expectedCars)
        {

            return CheckBodyTable(expectedCars, tableOfCarsBy);
        }

        public void AddCarToRentingCart(string carmodel)
        {
            WaitForBeingClickable(By.Id("carToRent_" + carmodel));

            _driver.FindElement(By.Id("carToRent_" + carmodel)).Click();
        }

        public void RemoveCarFromRentingCart(string carmodel)
        {
            WaitForBeingClickable(By.Id("removeCar_" + carmodel));
            _driver.FindElement(By.Id("removeCar_" + carmodel)).Click();
        }

        public bool RentingNotAvailable()
        {
            //the button is not Displayed=hidden

            return _driver.FindElement(buttonRentCar).Displayed == false;
        }
        public void RentCars()
        {
            WaitForBeingClickable(buttonRentCar);
            _driver.FindElement(buttonRentCar).Click();
        }
        public bool CheckMessageError(string error)
        {
            return _driver.PageSource.Contains(error);
        }
        public bool CheckMessageErrorNotAvailableCars(string expectedError)
        {
            return _driver.PageSource.Contains(expectedError);
        }

    }
}
