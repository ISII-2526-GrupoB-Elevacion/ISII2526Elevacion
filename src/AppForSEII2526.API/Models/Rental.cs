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
    }
}
