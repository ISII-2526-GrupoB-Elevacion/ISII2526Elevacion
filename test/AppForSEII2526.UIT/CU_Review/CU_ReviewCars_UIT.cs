using AppForSEII2526.UIT.CU_Purchase;
using AppForSEII2526.UIT.CU_Review;
using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading; // Necesario para Thread.Sleep
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Review
{
    public class CU_ReviewCars_UIT : UC_UIT
    {
        // IDs alineados con la lógica que funciona
        private const int carId1 = 1;
        private const string carModel1 = "Audi A4";
        private const string carClass1 = "Berlina";
        private const string carManufacturer1 = "Audi";
        private const string carFuelType1 = "Gasolina";
        private const string carColor1 = "Gris";

        private const int carId2 = 2;
        private const string carModel2 = "Toyota Corolla";
        private const string carClass2 = "Familiar";
        private const string carManufacturer2 = "Toyota";
        private const string carFuelType2 = "Diesel";
        private const string carColor2 = "Rojo";

        private const string name = "Elena";
        private const string surname = "Navarro Martínez";
        private const string country = "Spain";
        private const string driverType = "Experto";

        // Datos de la Reseña
        private const string descriptionValid = "Reseña para";
        private const int ratingValid = 5;

        // Page Objects
        private SelectCarsForReview_PO selectCarsForReview_PO;
        private CreateReview_PO createReview_PO;

        public CU_ReviewCars_UIT(ITestOutputHelper output) : base(output)
        {
            selectCarsForReview_PO = new SelectCarsForReview_PO(_driver, _output);
            createReview_PO = new CreateReview_PO(_driver, _output);
        }

        private void Precondition_perform_login()
        {
            Perform_login("elena@uclm.es", "Password1234%");
        }
        private void InitialStepsForReviewCars()
        {
            Precondition_perform_login();

            Thread.Sleep(1000);

            selectCarsForReview_PO.WaitForBeingVisible(By.Id("CreateReview"));
            _driver.FindElement(By.Id("CreateReview")).Click();
        }

        [Theory]
        [InlineData(carModel1, carClass1, carManufacturer1, carFuelType1, carColor1, "Audi", "")]
        [InlineData(carModel2, carClass2, carManufacturer2, carFuelType2, carColor2, "", "Diesel")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_AF0_UC4_2_3_Filtering(string model, string carClass, string manufacturer, string fuelType, string color, string filterManufacturer, string filterFuelType)
        {
            //Arrange
            InitialStepsForReviewCars();
            var expectedCars = new List<string[]> {
                new string[] { model, carClass, manufacturer, color, fuelType }
            };

            //Act
            selectCarsForReview_PO.SearchCars(filterManufacturer, filterFuelType);

            //Assert
            Assert.True(selectCarsForReview_PO.CheckListOfCars(expectedCars));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_AF1_UC4_4_ReviewNotAvailable()
        {
            //Arrange
            InitialStepsForReviewCars();

            //Act
            selectCarsForReview_PO.AddCarToReviewCart(carModel1);
            selectCarsForReview_PO.RemoveCarFromReviewCart(carModel1);

            //Assert
            Assert.True(selectCarsForReview_PO.ReviewNotAvailable());
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_AF2_UC4_5_10_DeleteSelectedCars()
        {
            //Arrange
            InitialStepsForReviewCars();

            //Act
            selectCarsForReview_PO.AddCarToReviewCart(carModel1);
            selectCarsForReview_PO.AddCarToReviewCart(carModel2);
            selectCarsForReview_PO.ReviewCars();

            createReview_PO.FillInReviewInfo(name, surname, country, driverType);

            createReview_PO.PressModifyCars();

            selectCarsForReview_PO.RemoveCarFromReviewCart(carModel2);

            selectCarsForReview_PO.ReviewCars();

            var expectedReviewItems = new List<string[]> {
                new string[] { carModel1, carFuelType1, carManufacturer1, carColor1 }
            };

            Assert.True(createReview_PO.CheckListOfReviewItems(expectedReviewItems));
        }

        [Theory]
        [InlineData("", surname, country, driverType, descriptionValid, ratingValid, "The field Name must be a string with a minimum length of 2 and a maximum length of 20.")]
        [InlineData(name, surname, " ", driverType, descriptionValid, ratingValid, "The field Country must be a string with a minimum length of 3 and a maximum length of 30.")]
        [InlineData(name, surname, country, " ", descriptionValid, ratingValid, "The field DriverType must be a string with a minimum length of 3 and a maximum length of 30.")]
        [InlineData(name, surname, country, driverType, descriptionValid, -1, "The field Rating must be between 1 and 5.")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_AF3_UC4_6_7_8_9_Testing_Errors_Mandatory_Data(string name, string surname, string country, string driverType, string description, int rating, string expectedMessageError)
        {
            //Arrange
            InitialStepsForReviewCars();

            //Act
            selectCarsForReview_PO.AddCarToReviewCart(carModel1);
            selectCarsForReview_PO.ReviewCars();

            createReview_PO.FillInReviewInfo(name, surname, country, driverType);

            // Si el ID es correcto (1) y el page object espera visibilidad, esto funcionará
            createReview_PO.FillInCarDetails(description, rating, carModel1);

            createReview_PO.PressReviewYourCars();

            //Assert
            Assert.True(createReview_PO.CheckValidationError(expectedMessageError), $"Expected error: {expectedMessageError}");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_AF4_10ModifyCars()
        {
            //Arrange
            InitialStepsForReviewCars();

            //Act
            selectCarsForReview_PO.AddCarToReviewCart(carModel1);
            selectCarsForReview_PO.AddCarToReviewCart(carModel2);
            selectCarsForReview_PO.ReviewCars();

            createReview_PO.FillInReviewInfo(name, surname, country, driverType);
            createReview_PO.FillInCarDetails(descriptionValid, ratingValid, carModel1);
            createReview_PO.FillInCarDetails(descriptionValid, ratingValid, carModel2);

            createReview_PO.PressModifyCars();

            selectCarsForReview_PO.RemoveCarFromReviewCart(carModel2);
            selectCarsForReview_PO.ReviewCars();

            //createReview_PO.FillInCarDetails(descriptionValid, ratingValid, carModel1);

            //Assert
            var expectedReviewItems = new List<string[]> {
                new string[] { carModel1, carFuelType1, carManufacturer1, carColor1 }
            };
            Assert.True(createReview_PO.CheckListOfReviewItems(expectedReviewItems));
        }
    }
}