namespace AppForSEII2526.API.DTOs.ReviewDTO
{
    public class ReviewItemDTO
    {
        [StringLength(20, ErrorMessage = "Model name cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string Model { get; set; }

        [StringLength(15, ErrorMessage = "FuelType cannot be any longer than 15 characters, neither shorter than 1.", MinimumLength = 1)]
        public string FuelType { get; set; }

        [StringLength(20, ErrorMessage = "Manufacturer cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string Manufacturer { get; set; }

        [StringLength(20, ErrorMessage = "Color cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string Color { get; set; }

        [Range(1, 5, ErrorMessage = "Minimum is 1 and maximum 5")]
        public float Rating { get; set; }


        public string ? Description { get; set; }

        public ReviewItemDTO() { }

        public ReviewItemDTO(string model, string manufacturer, string color, float rating, string? description)
        {
            Model = model;
            Manufacturer = manufacturer;
            Color = color;
            Rating = rating;
            Description = description ?? "No se ha escrito ninguna descripción";
            FuelType = "fuelType";
        }

        public ReviewItemDTO(string model, string fuelType, string manufacturer, string color, float rating, string? description)
        {
            Model = model;
            FuelType = fuelType;
            Manufacturer = manufacturer;
            Color = color;
            Rating = rating;
            Description = description;
        }
    }
}
