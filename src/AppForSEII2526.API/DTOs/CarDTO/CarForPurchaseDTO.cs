namespace AppForSEII2526.API.DTOs.CarDTO
{
    public class CarForPurchaseDTO
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string FuelType { get; set; }
        public string Manufacturer { get; set; }
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