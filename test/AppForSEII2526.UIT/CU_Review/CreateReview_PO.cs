using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Review
{
    public class CreateReview_PO : PageObject
    {
        // Selectores estáticos
        private By _nameBy = By.Id("Name");
        private By _surnameBy = By.Id("Surname");
        private By _countryBy = By.Id("Country");
        private By _driverTypeBy = By.Id("DriverType");

        // Selectores de botones
        private By _reviewCarsBtnBy = By.Id("ReviewCars");
        private By _modifyCarsBtnBy = By.Id("ModifyCars");
        private By _tableReviewItemsBy = By.Id("TableOfReviewItems");

        // Elementos web (helpers)
        private IWebElement _name() => _driver.FindElement(_nameBy);
        private IWebElement _surname() => _driver.FindElement(_surnameBy);
        private IWebElement _country() => _driver.FindElement(_countryBy);
        private IWebElement _driverType() => _driver.FindElement(_driverTypeBy);

        public CreateReview_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        // Rellena la información del conductor
        public void FillInReviewInfo(string name, string surname, string country, string driverType)
        {
            // Esperamos a que el primer campo sea visible para asegurar que la página cargó
            WaitForBeingVisible(_nameBy);

            _name().SendKeys(name);
            _surname().SendKeys(surname);
            _country().SendKeys(country);
            _driverType().SendKeys(driverType);
        }

        // Rellena la descripción y el rating del coche en la tabla
        public void FillInCarDetails(string description, int rating, int carId)
        {
            // Definimos los selectores dinámicos basados en el ID del coche
            By descriptionBy = By.Id("Description_" + carId);
            By ratingBy = By.Id("Rating_" + carId);

            WaitForBeingVisible(descriptionBy);

            _driver.FindElement(descriptionBy).SendKeys(description);

            var ratingInput = _driver.FindElement(ratingBy);
            ratingInput.Clear();
            ratingInput.SendKeys(rating.ToString());
        }

        // Botón "Review your cars" (Confirmar)
        public void PressReviewYourCars()
        {
            WaitForBeingVisible(_reviewCarsBtnBy);
            _driver.FindElement(_reviewCarsBtnBy).Click();
        }

        // Botón "Modify cars" (Volver atrás)
        public void PressModifyCars()
        {
            WaitForBeingVisible(_modifyCarsBtnBy);
            _driver.FindElement(_modifyCarsBtnBy).Click();
        }

        // Chequeo de la tabla resumen
        public bool CheckListOfReviewItems(List<string[]> expectedReviewItems)
        {
            // Esperamos a que la tabla de resultados se renderice
            WaitForBeingVisible(_tableReviewItemsBy);
            return CheckBodyTable(expectedReviewItems, _tableReviewItemsBy);
        }

        // Verificación de errores de validación
        public bool CheckValidationError(string expectedError)
        {
            return _driver.PageSource.Contains(expectedError);
        }
    }
}