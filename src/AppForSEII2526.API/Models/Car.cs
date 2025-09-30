namespace AppForSEII2526.API.Models
{
    public class Car
    {
        public string CarClass { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public float PurchasingPrice { get; set; }
        public int QuantityForPurchasing { get; set; }
        public int QuantityForRenting { get; set; }
        public int RentalItems { get; set; }
        public float RentingPrice { get; set; }
        public int ReviewItems { get; set; }
        public string EngDisplacement { get; set; }
        public string FuelType { get; set; }
        public string MaintenanceTypes { get; set; }
        public int PurchaseItems { get; set; }
        public int RimSize { get; set; }
    }
}
