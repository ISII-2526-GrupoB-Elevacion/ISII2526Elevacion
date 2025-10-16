namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(CarId), nameof(PurchaseId))]
    public class PurchaseItem
    {
        public int CarId { get; set; }
        public Car Car { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for Purchase is 1")]
        public int Quantity { get; set; }

        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }
    }
}