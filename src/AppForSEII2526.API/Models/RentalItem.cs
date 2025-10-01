namespace AppForSEII2526.API.Models
{
    public class RentalItem
    {
        [Key]
        public int CarId { get; set; }
        public int Quantity { get; set; }
        [Key]
        public int RentalId { get; set; }
    }
}
