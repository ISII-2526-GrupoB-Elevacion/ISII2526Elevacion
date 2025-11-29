using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class PurchaseStateContainer
    {
        //creamos una instancia de Purchase cuando una instancia de PurchaseStateContainer es creada
        public PurchaseForCreateDTO Purchase { get; private set; } = new PurchaseForCreateDTO()
        {
            PurchaseItems = new List<PurchaseItemDTO>()
        };

        //calculamos el precio total de los coches que queremos comprar
        public decimal TotalPrice
        {
            get
            {
                return Convert.ToDecimal(Purchase.PurchaseItems.Sum(pi => pi.PurchasingPrice));
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();


        public void AddCarToPurchase(CarForPurchaseDTO car)
        {
            //before adding a car we checked whether it has been already added
            if (!Purchase.PurchaseItems.Any(pi => pi.Model == car.Model))
                //we add it if it is not in the list
                Purchase.PurchaseItems.Add(new PurchaseItemDTO()
                {
                    Model = car.Model,
                    PurchasingPrice = (float)car.PurchasingPrice,
                    Color = car.Color
                }
            );
        }

        //to delete movies from the list of selected movies
        public void RemoveRentalItemToRent(PurchaseItemDTO item)
        {
            Purchase.PurchaseItems.Remove(item);

        }

        //we eliminate all the movies from the list
        public void ClearRentingCart()
        {
            Purchase.PurchaseItems.Clear();

        }

        //we have already finished the process of renting, thus, we create a new Rental 
        public void RentalProcessed()
        {
            //we have finished the rental process so we create a new object without data
            Purchase = new PurchaseForCreateDTO()
            {
                PurchaseItems = new List<PurchaseItemDTO>()
            };
        }
    }
}
