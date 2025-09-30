namespace AppForSEII2526.API.Models
{
    public class Rental
    {
        public string DeliveryCarDealer { get; set; }
        public DateTime EndDate { get; set; }
        public int Id { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime RentingDate { get; set; }
        public DateTime StartDate { get; set; }
        public float TotalPrice { get; set; }
    }
}
