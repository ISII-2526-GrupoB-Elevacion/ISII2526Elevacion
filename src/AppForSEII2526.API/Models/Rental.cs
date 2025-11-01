namespace AppForSEII2526.API.Models
{
    public class Rental
    {
        [StringLength(20,ErrorMessage ="DeliveryCarDealer cannot be any longer than 20 characters, neither shorter than 1.",MinimumLength =1)]
        public string DeliveryCarDealer { get; set; }
        
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        
        [Key]
        public int Id { get; set; }
        
        public PaymentMethodEnum PaymentMethod { get; set; }
        
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RentingDate { get; set; }
        
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }


        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(30,2000, ErrorMessage = "Minimum is 300 and maximum 2000")]
        public float TotalPrice { get; set; }

        public IList<RentalItem> RentalItems { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public enum PaymentMethodEnum
        {
            Visa,
            GooglePay,
            PayPal
        }
        public Rental()
        {

        }

        public Rental(string deliveryCarDealer, DateTime endDate, PaymentMethodEnum paymentMethod, DateTime rentingDate, DateTime startDate, float totalPrice, IList<RentalItem> rentalItems,ApplicationUser applicationUser)
        {
            DeliveryCarDealer = deliveryCarDealer;
            EndDate = endDate;
            PaymentMethod = paymentMethod;
            RentingDate = rentingDate;
            StartDate = startDate;
            TotalPrice = totalPrice;
            RentalItems = rentalItems;
            ApplicationUser = applicationUser;
        }

    }
}
