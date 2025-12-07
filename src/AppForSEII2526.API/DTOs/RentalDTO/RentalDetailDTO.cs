using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.RentalDTO
{
    public class RentalDetailDTO
    {
        public int Id { get; set; }

        [StringLength(20, ErrorMessage = "Name cannot be any longer than 20 characters, neither shorter than 2.", MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "Surname cannot be any longer than 100 characters, neither shorter than 4.", MinimumLength = 4)]
        public string Surname { get; set; }

        [StringLength(100, ErrorMessage = "UserName cannot be any longer than 100 characters, neither shorter than 4.", MinimumLength = 4)]
        public string UserName { get; set; }

        [StringLength(50, ErrorMessage = "DeliveryCarDealer cannot be any longer than 50 characters, neither shorter than 1.", MinimumLength = 1)]
        public string DeliveryCarDealer { get; set; }

        public Rental.RentalPaymentMethodEnum PaymentMethod { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RentingDate { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(30, 2000, ErrorMessage = "Minimum is 300 and maximum 2000")]
        public float TotalPrice { get; set; }

        public IList<RentalItemDTO> RentalItems { get; set; }

        public RentalDetailDTO(int id,  string name, string surname, string userName, string deliveryCarDealer, Rental.RentalPaymentMethodEnum paymentMethod, DateTime startDate, DateTime endDate, DateTime rentingDate, float totalPrice, IList<RentalItemDTO> rentalItems)
        {
            Id = id;
            Name = name;
            Surname = surname;
            UserName = userName;
            DeliveryCarDealer = deliveryCarDealer;
            PaymentMethod = paymentMethod;
            StartDate = startDate;
            EndDate = endDate;
            RentingDate = rentingDate;
            TotalPrice = totalPrice;
            RentalItems = rentalItems;
        }

        public override bool Equals(object? obj)
        {
            return obj is RentalDetailDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Surname == dTO.Surname &&
                   UserName == dTO.UserName &&
                   DeliveryCarDealer == dTO.DeliveryCarDealer &&
                   PaymentMethod == dTO.PaymentMethod &&
                   StartDate == dTO.StartDate &&
                   EndDate == dTO.EndDate &&
                   RentingDate == dTO.RentingDate &&
                   TotalPrice == dTO.TotalPrice &&
                   RentalItems.SequenceEqual(dTO.RentalItems);
        }
    }
}
