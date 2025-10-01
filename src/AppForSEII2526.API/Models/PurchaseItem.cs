namespace AppForSEII2526.API.Models
{
    public class PurchaseItem
    {
        [Key]
        public int CarId { get; set; }
        [Key]
        public int PurchaseId { get; set; }
        public int Quantity { get; set; }
    }
}