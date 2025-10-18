namespace AppForSEII2526.API.DTOs.PurchaseDTO
{
    public class PurchaseItemDTO
    {
        public string Model { get; set; }
        public float PurchasingPrice { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }

        public PurchaseItemDTO(string model, int quantity, float purchasingPrice, string color)
        {
            Model = model;
            Quantity = quantity;
            PurchasingPrice = purchasingPrice;
            Color = color;
        }
    }
}
