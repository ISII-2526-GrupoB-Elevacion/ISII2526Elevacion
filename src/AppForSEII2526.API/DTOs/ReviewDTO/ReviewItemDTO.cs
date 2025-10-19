namespace AppForSEII2526.API.DTOs.ReviewDTO
{
    public class ReviewItemDTO
    {
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string Color { get; set; }

        public float Rating { get; set; }
        public string ? Description { get; set; }

        public ReviewItemDTO(string model, string manufacturer, string color, float rating, string? description)
        {
            Model = model;
            Manufacturer = manufacturer;
            Color = color;
            Rating = rating;
            Description = description;
        }
    }
}
