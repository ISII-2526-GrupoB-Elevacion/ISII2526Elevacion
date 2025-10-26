using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.PurchaseDTO
{
    public class PurchaseForCreateDTO
    {
        public float PurchasingPrice { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
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
