using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.PurchaseDTO
{
    public class PurchaseItemDTO
    {
        public string Model { get; set; }
        public float PurchasingPrice { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }

        public PurchaseItemDTO()
        {

        }

        public PurchaseItemDTO(string model, int quantity, float purchasingPrice, string color)
        {
            Model = model;
            PurchasingPrice = purchasingPrice;
            Color = color;
            Quantity = quantity;
        }

        public PurchaseItemDTO(string model, float purchasingPrice, string color, int quantity, string description)
        {
            Model = model;
            PurchasingPrice = purchasingPrice;
            Color = color;
            Quantity = quantity;
            Description = description;
        }
    }
}
