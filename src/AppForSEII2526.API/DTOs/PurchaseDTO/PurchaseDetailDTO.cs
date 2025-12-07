
namespace AppForSEII2526.API.DTOs.PurchaseDTO
{
    public class PurchaseDetailDTO
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

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PurchasingDate { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(3000, 100000, ErrorMessage = "Minimum is 3000 and maximum 100000")]
        public float PurchasingPrice { get; set; }

        public IList<PurchaseItemDTO> PurchaseItems { get; set; }

        public PurchaseDetailDTO(int id,  string name, string surname, string userName, string deliveryCarDealer, DateTime purchasingDate, float purchasingPrice, IList<PurchaseItemDTO> purchaseItems)
        {
            Id = id;
            Name = name;
            Surname = surname;
            UserName = userName;
            DeliveryCarDealer = deliveryCarDealer;
            PurchasingDate = purchasingDate;
            PurchasingPrice = purchasingPrice;
            PurchaseItems = purchaseItems;
        }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseDetailDTO dTO &&
                   Name == dTO.Name &&
                   Surname == dTO.Surname &&
                   DeliveryCarDealer == dTO.DeliveryCarDealer &&
                   PurchasingDate == dTO.PurchasingDate &&
                   PurchasingPrice == dTO.PurchasingPrice &&
                   PurchaseItems.SequenceEqual(dTO.PurchaseItems);
        }
    }
}
