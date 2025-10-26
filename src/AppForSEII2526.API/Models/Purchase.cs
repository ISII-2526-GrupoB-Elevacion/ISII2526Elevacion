namespace AppForSEII2526.API.Models
{
    public class Purchase
    {
        [StringLength(20, ErrorMessage = "DeliveryCarDealer cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string DeliveryCarDealer { get; set; }

        [Key]
        public int Id { get; set; }

        public PurchasePaymentMethodEnum PaymentMethod { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PurchasingDate { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(3000, 100000, ErrorMessage = "Minimum is 3000 and maximum 100000")]
        public float PurchasingPrice { get; set; }

        public IList<PurchaseItem> PurchaseItems { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public enum PurchasePaymentMethodEnum
        {
            GooglePay,
            Visa
        }
    }
}
