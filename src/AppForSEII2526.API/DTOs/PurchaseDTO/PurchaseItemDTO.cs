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

        public string? Description { get; set; }

        public PurchaseItemDTO()
        {

        }

        /*Aquí hay 2 constructores porque uno es para el post y otro es para el detail,
        como el detail no tiene que imprimir la descripción, se pone como campo por defecto "description" y
        no se le pasa ningún string relacionado con esto*/
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

        public override bool Equals(object? obj)
        {
            return obj is PurchaseItemDTO dTO &&
                   Model == dTO.Model &&
                   PurchasingPrice == dTO.PurchasingPrice &&
                   Color == dTO.Color &&
                   Quantity == dTO.Quantity &&
                   Description == dTO.Description;
        }
    }
}
