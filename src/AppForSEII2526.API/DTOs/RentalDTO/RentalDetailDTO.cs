using AppForSEII2526.API.Models;
namespace AppForSEII2526.API.DTOs.RentalDTO
{
    public class RentalDetailDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public Rental.RentalPaymentMethodEnum PaymentMethod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RentingDate { get; set; }
        public float TotalPrice { get; set; }
        public IList<RentalItemDTO> RentalItems { get; set; }

        public RentalDetailDTO(string name, string surname, Rental.RentalPaymentMethodEnum paymentMethod, DateTime startDate, DateTime endDate, DateTime rentingDate, float totalPrice, IList<RentalItemDTO> rentalItems)
        {
            Name = name;
            Surname = surname;
            PaymentMethod = paymentMethod;
            StartDate = startDate;
            EndDate = endDate;
            RentingDate = rentingDate;
            TotalPrice = totalPrice;
            RentalItems = rentalItems;
        }
    }
}
