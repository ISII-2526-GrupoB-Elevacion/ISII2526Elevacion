using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.CU_Review
{
    public class CreateReview_PO : PageObject
    {
        // Selectores basados en la imagen "Create Review"
        private By _nameBy = By.Id("Name");
        private By _surnameBy = By.Id("Surname");
        private By _countryBy = By.Id("Country");
        private By _driverTypeBy = By.Id("DriverType");

        // Elementos web
        private IWebElement _name() => _driver.FindElement(_nameBy);
        private IWebElement _surname() => _driver.FindElement(_surnameBy);
        private IWebElement _country() => _driver.FindElement(_countryBy);
        private IWebElement _driverType() => _driver.FindElement(_driverTypeBy);

        public CreateReview_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        // Adaptado: Rellena la información del conductor (4 campos)
        public void FillInReviewInfo(string name, string surname, string country, string driverType)
        {
            WaitForBeingVisible(_nameBy);
            _name().SendKeys(name);
            _surname().SendKeys(surname);
            _country().SendKeys(country);
            _driverType().SendKeys(driverType);
        }

        // Adaptado: Rellena la descripción y el rating del coche en la tabla
        // Se asume que los IDs siguen el patrón "Description_{id}" y "Rating_{id}"
        public void FillInCarDetails(string description, int rating, int carId)
        {
            _driver.FindElement(By.Id("Description_" + carId)).SendKeys(description);

            var ratingInput = _driver.FindElement(By.Id("Rating_" + carId));
            ratingInput.Clear();
            ratingInput.SendKeys(rating.ToString());
        }

        // Adaptado: Botón "Review your cars"
        public void PressReviewYourCars()
        {
            // Asumo que el ID es 'ReviewCars' o 'Submit' basándome en el código anterior
            _driver.FindElement(By.Id("ReviewCars")).Click();
        }

        // Adaptado: Botón "Modify cars" (visible en la imagen)
        public void PressModifyCars()
        {
            _driver.FindElement(By.Id("ModifyCars")).Click();
        }

        // Chequeo de la tabla (columnas: Model, FuelType, Manufacturer, Color, Description, Rating)
        public bool CheckListOfReviewItems(List<string[]> expectedReviewItems)
        {
            return CheckBodyTable(expectedReviewItems, By.Id("TableOfReviewItems"));
        }

        public bool CheckValidationError(string expectedError)
        {
            return _driver.PageSource.Contains(expectedError);
        }
    }
}