using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Rental
{
    internal class CreateRental_PO: PageObject
    {
        private By Name = By.Id("RentalName");
        private By Surname = By.Id("RentalSurname");
        private By DeliveryCarDealer = By.Id("RentalDeliveryCarDealer");
        private By PaymenMethod = By.Id("RentalPaymentMethod");
        private By TotalPrice = By.Id("RentalTotalPrice");

        private IWebElement _name() => _driver.FindElement(Name);
        private IWebElement _surname() => _driver.FindElement(Surname);
        private IWebElement _deliveryCarDealer() => _driver.FindElement(DeliveryCarDealer);
        private IWebElement _paymentMethod() => _driver.FindElement(PaymenMethod);
        public CreateRental_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {

        }

        public void FillInPurchaseQuantity(string quantity, string model)
        {
            WaitForBeingClickable(By.Id("quantity_" + model));
            _driver.FindElement(By.Id("quantity_" + model)).Clear();
            _driver.FindElement(By.Id("quantity_" + model)).SendKeys(quantity);
        }

        public void FillInRentalInfo(string name, string surname, string deliveryCarDealer, string paymentMethod)
        {
            WaitForBeingVisible(Name);
            WaitForBeingVisible(Surname);
            WaitForBeingVisible(DeliveryCarDealer);
            _name().SendKeys(name);
            _surname().SendKeys(surname);
            _deliveryCarDealer().SendKeys(deliveryCarDealer);

            //create select element object 
            SelectElement selectElement = new SelectElement(_paymentMethod());

            //select Action from the dropdown menu
            selectElement.SelectByText(paymentMethod);
        }

        public void PressRentYourCars()
        {
            _driver.FindElement(By.Id("RentalSubmit")).Click();
        }
        public void PressModifyCars()
        {
            _driver.FindElement(By.Id("ModifyRental")).Click();
        }

        public bool CheckListOfRentalItems(List<string[]> expectedRentalItems)
        {
            return CheckBodyTable(expectedRentalItems, By.Id("TableOfRentalItems"));
        }

        public bool CheckTotalPrice(string expectedTotalPrice)
        {
            return _driver.FindElement(TotalPrice).GetAttribute("value").Equals(expectedTotalPrice);
        }

        public bool CheckValidationError(string expectedError)
        {
            return _driver.PageSource.Contains(expectedError);
        }
    }
}
