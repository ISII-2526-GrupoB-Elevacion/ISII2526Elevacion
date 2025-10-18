namespace AppForSEII2526.API.DTOs.PurchaseDTO
{
    public class PurchaseDetailDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime PurchasingDate { get; set; }
        public float PurchasingPrice { get; set; }
        public IList<PurchaseItemDTO> PurchaseItems { get; set; }

        public PurchaseDetailDTO(string name, string surname, DateTime purchasingDate, float purchasingPrice, IList<PurchaseItemDTO> purchaseItems)
        {
            Name = name;
            Surname = surname;
            PurchasingDate = purchasingDate;
            PurchasingPrice = purchasingPrice;
            PurchaseItems = purchaseItems;
        }
    }
}
