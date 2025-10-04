namespace AppForSEII2526.API.Models
{
    public class PurchaseItem
    {
        [Key]
        public int CarId { get; set; }
        [Key]
        public int PurchaseId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for Purchase is 1")]
        public int Quantity { get; set; }
    }
}