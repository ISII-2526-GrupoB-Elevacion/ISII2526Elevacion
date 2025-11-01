namespace AppForSEII2526.API.Models
{
    public class Car
    {
        [StringLength(20, ErrorMessage = "CarClass cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string CarClass { get; set; }

        [StringLength(20, ErrorMessage = "Color cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string Color { get; set; }

        [StringLength(100, ErrorMessage = "Description cannot be any longer than 100 characters, neither shorter than 10.", MinimumLength = 10)]
        public string Description { get; set; }

        [Key]
        public int Id { get; set; }

        [StringLength(20, ErrorMessage = "Manufacturer cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string Manufacturer { get; set; }


        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(3000, 20000, ErrorMessage = "Minimum is 3000 and maximum 20000")]
        public float PurchasingPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for Purchase is 1")]
        public int QuantityForPurchasing { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for Renting is 1")]
        public int QuantityForRenting { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(30, 200, ErrorMessage = "Minimum Renting Prices is 30 and maximum 200")]
        public float RentingPrice { get; set; }

        [StringLength(10, ErrorMessage = "EngDisplacement cannot be any longer than 10 characters, neither shorter than 1.", MinimumLength = 1)]
        public string EngDisplacement { get; set; }

        [StringLength(15, ErrorMessage = "FuelType cannot be any longer than 15 characters, neither shorter than 1.", MinimumLength = 1)]
        public string FuelType { get; set; }

        [StringLength(20, ErrorMessage = "MainteannceType cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string MaintenanceTypes { get; set; }

        [Range(20, 60, ErrorMessage = "Minimum Rim size is 20 and maximum 60")]
        public float RimSize { get; set; }

        public Model Model { get; set; }

        public IList<RentalItem> RentalItems { get; set; }

        public IList<PurchaseItem> PurchaseItems { get; set; }

        public IList<ReviewItem> ReviewItems { get; set; }

        public Car()
        {

        }

        public Car(string carClass, string color, string description, string manufacturer, float purchasingPrice, int quantityForPurchasing, int quantityForRenting, float rentingPrice, string engDisplacement, string fuelType, string maintenanceTypes, float rimSize, Model model)
        {
            CarClass = carClass;
            Color = color;
            Description = description;
            Manufacturer = manufacturer;
            PurchasingPrice = purchasingPrice;
            QuantityForPurchasing = quantityForPurchasing;
            QuantityForRenting = quantityForRenting;
            RentingPrice = rentingPrice;
            EngDisplacement = engDisplacement;
            FuelType = fuelType;
            MaintenanceTypes = maintenanceTypes;
            RimSize = rimSize;
            Model = model;
        }
    }
}
