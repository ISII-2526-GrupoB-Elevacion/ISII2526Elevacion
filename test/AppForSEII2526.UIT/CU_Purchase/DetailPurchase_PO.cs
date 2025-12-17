using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Purchase
{
    public class DetailPurchase_PO : PageObject
    {
        public DetailPurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {

        }

        public bool CheckPurchaseDetail(string nameSurname, string deliveryCarDealer, DateTime purchasingDate, string totalPrice)
        {
            Thread.Sleep(750);
            WaitForBeingVisible(By.Id("TotalPrice"));
            bool result = true;
            result = result && _driver.FindElement(By.Id("NameSurname")).Text.Contains(nameSurname);
            result = result && _driver.FindElement(By.Id("DeliveryCarDealer")).Text.Contains(deliveryCarDealer);
            result = result && _driver.FindElement(By.Id("TotalPrice")).Text.Contains(totalPrice);

            var actualPurchaseDate = DateTime.Parse(_driver.FindElement(By.Id("PurchasingDate")).Text);
            result = result && ((actualPurchaseDate - purchasingDate) < new TimeSpan(0, 1, 0));

            return result;
        }

        public bool CheckListOfPurchase(List<string[]> expectedPurchaseItems)
        {
            return CheckBodyTable(expectedPurchaseItems, By.Id("PurchasedCars"));
        }
    }
}
