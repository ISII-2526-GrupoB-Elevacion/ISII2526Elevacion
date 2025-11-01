namespace AppForSEII2526.API.DTOs.CarDTO
{
    public class CarForPurchaseDTO
    {
        public int Id { get; set; }

        [StringLength(20, ErrorMessage = "Model name cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string Model { get; set; }

        [StringLength(20, ErrorMessage = "Color cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string Color { get; set; }

        [StringLength(15, ErrorMessage = "FuelType cannot be any longer than 15 characters, neither shorter than 1.", MinimumLength = 1)]
        public string FuelType { get; set; }

        [StringLength(20, ErrorMessage = "Manufacturer cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string Manufacturer { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(3000, 20000, ErrorMessage = "Minimum is 3000 and maximum 20000")]
        public float PurchasingPrice { get; set; }

        public CarForPurchaseDTO(int id, string model, string color, string fuelType, string manufacturer, float purchasingPrice)
        {
            Id = id;
            Model = model;
            Color = color;
            FuelType = fuelType;
            Manufacturer = manufacturer;
            PurchasingPrice = purchasingPrice;
        }

        public override bool Equals(object? obj)
        {
            return obj is CarForPurchaseDTO dTO &&
                   Id == dTO.Id &&
                   Model == dTO.Model &&
                   Color == dTO.Color &&
                   FuelType == dTO.FuelType &&
                   Manufacturer == dTO.Manufacturer &&
                   PurchasingPrice == dTO.PurchasingPrice;
        }
    }
}