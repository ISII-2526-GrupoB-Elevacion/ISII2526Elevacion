namespace AppForSEII2526.API.DTOs.ReviewDTO
{
    public class ReviewItemDTO
    {
        public string Model { get; set; }
        public string FuelType { get; set; }
        public string Manufacturer { get; set; }
        public string Color { get; set; }
        public float Rating { get; set; }
        public string ? Description { get; set; }

        public ReviewItemDTO() { }

        public ReviewItemDTO(string model, string manufacturer, string color, float rating, string? description)
        {
            Model = model;
            Manufacturer = manufacturer;
            Color = color;
            Rating = rating;
            Description = description;
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
