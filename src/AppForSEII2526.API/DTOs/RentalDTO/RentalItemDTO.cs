namespace AppForSEII2526.API.DTOs.RentalDTO
{
    public class RentalItemDTO
    {
        [StringLength(20, ErrorMessage = "Model name cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string Model { get; set; }

        [StringLength(20, ErrorMessage = "Manufacturer cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string Manufacturer { get; set; }

        public float RentingPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for Renting is 1")]
        public int Quantity { get; set; }

        public RentalItemDTO(string model, string manufacturer, float rentingPrice, int quantity)
        {
            Model = model;
            Manufacturer = manufacturer;
            RentingPrice = rentingPrice;
            Quantity = quantity;
        }
    }
}
