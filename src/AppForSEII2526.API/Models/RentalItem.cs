namespace AppForSEII2526.API.Models
{
    public class RentalItem
    {
        [Key]
        public int CarId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for Renting is 1")]
        public int Quantity { get; set; }
        
        [Key]
        public int RentalId { get; set; }
    }
}
