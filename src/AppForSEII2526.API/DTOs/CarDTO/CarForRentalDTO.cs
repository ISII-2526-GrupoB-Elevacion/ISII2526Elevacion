namespace AppForSEII2526.API.DTOs.CarDTO
{
    public class CarForRentalDTO
    {
        

        public int Id { get; set; }

        [StringLength(20, ErrorMessage = "Model name cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string Model { get; set; }

        [StringLength(15, ErrorMessage = "FuelType cannot be any longer than 15 characters, neither shorter than 1.", MinimumLength = 1)]
        public string FuelType { get; set; }

        [StringLength(20, ErrorMessage = "Manufacturer cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string Manufacturer { get; set; }

        [Range(30, 200, ErrorMessage = "Minimum Renting Prices is 30 and maximum 200")]
        public float RentingPrice { get; set; }

        [StringLength(20, ErrorMessage = "Color cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
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
