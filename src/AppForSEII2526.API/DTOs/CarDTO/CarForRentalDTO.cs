namespace AppForSEII2526.API.DTOs.CarDTO
{
    public class CarForRentalDTO
    {
        

        public int Id { get; set; }
        public string Model { get; set; }
        public string FuelType { get; set; }
        public string Manufacturer { get; set; }
        public float RentingPrice { get; set; }
        public string Color { get; set; }
        public CarForRentalDTO(int id, string model, string fuelType, string manufacturer, float rentingPrice, string color)
        {
            Id = id;
            Model = model;
            FuelType = fuelType;
            Manufacturer = manufacturer;
            RentingPrice = rentingPrice;
            Color = color;
        }

    }
}
