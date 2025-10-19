namespace AppForSEII2526.API.DTOs.RentalDTO
{
    public class RentalItemDTO
    {
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public float RentingPrice { get; set; }
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
