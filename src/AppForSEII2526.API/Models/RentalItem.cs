namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(CarId),nameof(RentalId))]
    public class RentalItem
    {
        public int CarId { get; set; }
        public Car Car { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for Renting is 1")]
        public int Quantity { get; set; }

        public int RentalId { get; set; }
        public Rental Rental { get; set; }

        public RentalItem() { }

        public RentalItem(int carId, int quantity, Rental rental)
        {
            CarId = carId;
            Quantity = quantity;
            Rental = rental;
        }
    }
}
