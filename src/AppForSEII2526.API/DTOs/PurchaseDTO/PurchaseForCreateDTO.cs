using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.PurchaseDTO
{
    public class PurchaseForCreateDTO
    {
        public float PurchasingPrice { get; set; }

        [StringLength(20, ErrorMessage = "Name cannot be any longer than 20 characters, neither shorter than 2.", MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "Surname cannot be any longer than 100 characters, neither shorter than 4.", MinimumLength = 4)]
        public string Surname { get; set; }

        public string UserName { get; set; }

        [StringLength(20, ErrorMessage = "DeliveryCarDealer cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string DeliveryCarDealer { get; set; }

        public Purchase.PurchasePaymentMethodEnum PaymentMethod { get; set; }

        public IList<PurchaseItemDTO> PurchaseItems { get; set; }

        public PurchaseForCreateDTO(float purchasingPrice, string name, string surname, string userName, string deliveryCarDealer, Purchase.PurchasePaymentMethodEnum paymentMethod, IList<PurchaseItemDTO> purchaseItems)
        {
            PurchasingPrice = purchasingPrice;
            Name = name ?? throw new ArgumentNullException(nameof(Name));
            Surname = surname ?? throw new ArgumentNullException(nameof(Surname));
            UserName = userName ?? throw new ArgumentNullException(nameof(UserName));
            DeliveryCarDealer = deliveryCarDealer ?? throw new ArgumentNullException(nameof(DeliveryCarDealer));
            PaymentMethod = paymentMethod;
            PurchaseItems = purchaseItems ?? throw new ArgumentNullException(nameof(PurchaseItems));
        }
    }
}
