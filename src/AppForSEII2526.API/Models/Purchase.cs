namespace AppForSEII2526.API.Models
{
    public class Purchase
    {
        public string DeliveryCarDealer { get; set; }
        [Key]
        public int Id { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PurchasingDate { get; set; }
        public float PurchasingPrice { get; set; }
    }
}
