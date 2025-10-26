using AppForSEII2526.API.Models;
namespace AppForSEII2526.API.DTOs.RentalDTO
{
    public class RentalForCreateDTO
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname{ get; set; }
        public string DeliveryCarDealer { get; set; }
        public Rental.RentalPaymentMethodEnum PaymentMethod{ get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RentingDate { get; set; }
        public float TotalPrice { get; set; }
        public IList<RentalItemDTO> RentalItems { get; set; }

        public RentalForCreateDTO(string userName ,string name, string surname, string deliveryCarDealer, Rental.RentalPaymentMethodEnum paymentMethod, DateTime startDate, DateTime endDate, DateTime rentingDate, float totalPrice, IList<RentalItemDTO> rentalItems)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(UserName));
            Name = name ?? throw new ArgumentNullException(nameof(Name));
            Surname = surname ?? throw new ArgumentNullException(nameof(Surname));
            DeliveryCarDealer = deliveryCarDealer ?? throw new ArgumentNullException(nameof(DeliveryCarDealer));
            PaymentMethod = paymentMethod;
            StartDate = startDate;
            EndDate = endDate;
            RentingDate = rentingDate;
            TotalPrice = totalPrice;
            RentalItems = rentalItems ?? throw new ArgumentNullException(nameof(RentalItems));
        }
    }
}
