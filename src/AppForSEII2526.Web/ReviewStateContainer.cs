using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class ReviewStateContainer
    {
        //we create an instance of Rental when an instance of RentalStateContainer is created
        public ReviewForCreateDTO Review { get; private set; } = new ReviewForCreateDTO()
        {
            ReviewItems = new List<ReviewItemDTO>()
        };

        public event Action? OnChange;

        public void AddCarToReview(CarForReviewDTO car)
        {
            //before adding a movie we checked whether it has been already added
            if (!Review.ReviewItems.Any(ri => ri.Model == car.Model))
                //we add it if it is not in the list
                Review.ReviewItems.Add(new ReviewItemDTO()
                {
                    Model = car.Model,
                    Manufacturer = car.Manufacturer,
                    Color = car.Color,
                    FuelType = car.FuelType,
                }
            );

        }

        //to delete movies from the list of selected movies
        public void RemoveReviewItemToRent(ReviewItemDTO item)
        {
            Review.ReviewItems.Remove(item);

        }

        //we eliminate all the movies from the list
        public void ClearReviewCart()
        {
            Review.ReviewItems.Clear();

        }

        //we have already finished the process of renting, thus, we create a new Rental 
        public void ReviewProcessed()
        {
            //we have finished the rental process so we create a new object without data
            Review = new ReviewForCreateDTO()
            {
                ReviewItems = new List<ReviewItemDTO>()
            };
        }
    }
}
