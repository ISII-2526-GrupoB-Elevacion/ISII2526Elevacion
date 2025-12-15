using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Rental
{
    internal class DetailRental_PO: PageObject
    {
        public DetailRental_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckRentalDetail(string name, string surname, string delivery, string paymentmethod,
            DateTime rentalDate, DateTime from, DateTime to, string totalprice)
        {
            WaitForBeingVisible(By.Id("RentalTotalPrice"));
            bool result = true;
            result = result && _driver.FindElement(By.Id("Name")).Text.Contains(name);
            result = result && _driver.FindElement(By.Id("Surname")).Text.Contains(surname);
            result = result && _driver.FindElement(By.Id("DeliveryCarDealer")).Text.Contains(delivery);
            result = result && _driver.FindElement(By.Id("PaymentMethod")).Text.Contains(paymentmethod);
            result = result && _driver.FindElement(By.Id("RentalTotalPrice")).Text.Contains(totalprice);

            var actualRentalDate = DateTime.Parse(_driver.FindElement(By.Id("RentalDate")).Text);
            result = result && ((actualRentalDate - rentalDate) <= new TimeSpan(12, 0, 0));

            result = result && _driver.FindElement(By.Id("RentalPeriod")).Text.Contains($"{from.ToShortDateString()} - {to.ToShortDateString()}");

            return result;

        }

        public bool CheckListOfCars(List<string[]> expectedRentalItems)
        {
            return CheckBodyTable(expectedRentalItems, By.Id("RentedCars"));
        }
    }
}
