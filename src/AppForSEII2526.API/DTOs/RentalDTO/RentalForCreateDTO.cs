using AppForSEII2526.API.Models;
namespace AppForSEII2526.API.DTOs.RentalDTO
{
    public class RentalForCreateDTO
    {
        public string UserName { get; set; }

        [StringLength(20, ErrorMessage = "Name cannot be any longer than 20 characters, neither shorter than 2.", MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "Surname cannot be any longer than 100 characters, neither shorter than 4.", MinimumLength = 4)]
        public string Surname{ get; set; }

        [StringLength(20, ErrorMessage = "DeliveryCarDealer cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string DeliveryCarDealer { get; set; }

        public Rental.RentalPaymentMethodEnum PaymentMethod{ get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
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
