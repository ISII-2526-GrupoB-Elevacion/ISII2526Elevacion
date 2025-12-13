using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.CU_Purchase
{
    public class SelectCarsForPurchase_PO : PageObject
    {
        By carColor = By.Id("inputColor");
        By carModel = By.Id("inputModel");
        By buttonSearchCars = By.Id("searchCarsButton");
        By buttonPurchaseCars = By.Id("purchaseCarButton");
        By tableOfCarsBy = By.Id("TableOfCars");

        public SelectCarsForPurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {

        }

        public void SearchCars(string filterColor, string filterModel)
        {
            //wait for the webelement to be clickable
            WaitForBeingClickable(carColor);
            WaitForBeingClickable(carModel);
            _driver.FindElement(carColor).SendKeys(filterColor);
            _driver.FindElement(carModel).SendKeys(filterModel);
            _driver.FindElement(buttonSearchCars).Click();
        }

        public bool CheckListOfCars(List<string[]> expectedCars)
        {
            return CheckBodyTable(expectedCars, tableOfCarsBy);
        }

        public void AddCarToShoppingCart(string carModel)
        {
            WaitForBeingClickable(By.Id("carToPurchase_" + carModel));
            _driver.FindElement(By.Id("carToPurchase_" + carModel)).Click();;
        }

        public void RemoveCarFromShoppingCart(string carModel)
        {
            WaitForBeingClickable(By.Id("removeCar_" + carModel));
            _driver.FindElement(By.Id("removeCar_" + carModel)).Click();
        }

        public bool PurchasingNotAvailable()
        {
            return _driver.FindElement(buttonPurchaseCars).Displayed == false;
        }
    }
}