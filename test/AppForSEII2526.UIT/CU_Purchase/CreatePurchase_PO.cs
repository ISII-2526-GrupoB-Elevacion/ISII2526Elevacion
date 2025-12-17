using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Purchase
{
    public class CreatePurchase_PO : PageObject
    {
        private By inputName = By.Id("Nombre");
        private By inputApellidos = By.Id("Apellidos");
        private By inputDeliveryAddress = By.Id("DeliveryAddress");
        private By inputPaymentMethod = By.Id("PaymentMethod");
        private By totalPrice = By.Id("TotalPrice");

        private IWebElement nameElement() => _driver.FindElement(inputName);
        private IWebElement surnameElement() => _driver.FindElement(inputApellidos);
        private IWebElement deliveryElement() => _driver.FindElement(inputDeliveryAddress);
        private IWebElement paymentMethodElement() => _driver.FindElement(inputPaymentMethod);

        public CreatePurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {

        }

        public void FillInPurchaseInfo(string name, string surname, string deliveryCarDealer, string paymentMethod)
        {
            WaitForBeingClickable(inputName);
            nameElement().SendKeys(name);
            surnameElement().SendKeys(surname);
            deliveryElement().SendKeys(deliveryCarDealer);

            SelectElement selectPaymentMethod = new SelectElement(paymentMethodElement());
            selectPaymentMethod.SelectByText(paymentMethod);
        }

        public void FillInPurchaseQuantity(string quantity, string model)
        {
            WaitForBeingClickable(By.Id("quantity_" + model));
            _driver.FindElement(By.Id("quantity_" + model)).Clear();
            _driver.FindElement(By.Id("quantity_" + model)).SendKeys(quantity);
        }

        public void PressPurchaseYourCars()
        {
            _driver.FindElement(By.Id("Submit")).Click();
        }

        public void PressModifyCars()
        {
            _driver.FindElement(By.Id("ModifyCars")).Click();
        }

        public bool CheckListOfPurchaseItems(List<string[]> expectedPurchaseItems, string quantity, string model)
        {
            return CheckBodyTable(expectedPurchaseItems, By.Id("TableOfPurchaseItems")) && _driver.FindElement(By.Id("quantity_" + model)).GetAttribute("value")==quantity;
        }

        public bool CheckValidationError(string expectedError)
        {
            Thread.Sleep(500);
            return _driver.PageSource.Contains(expectedError);
        }
        
        public bool CheckTotalPrice(string expectedPrice)
        {
            WaitForBeingVisible(totalPrice);
            string actualPriceText = _driver.FindElement(totalPrice).Text;
            return actualPriceText.Contains(expectedPrice);
        }
    }
}
