using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.PurchaseDTO
{
    public class PurchaseItemDTO
    {
        [StringLength(20, ErrorMessage = "Model name cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string Model { get; set; }

        public float PurchasingPrice { get; set; }

        [StringLength(20, ErrorMessage = "Color cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string Color { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for Purchase is 1")]
        public int Quantity { get; set; }

        [StringLength(100, ErrorMessage = "Description cannot be any longer than 100 characters, neither shorter than 10.", MinimumLength = 10)]
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
            Description = "description";
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
