namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(CarId), nameof(ReviewId))]
    public class ReviewItem
    {
        public int CarId { get; set; }
        public Car Car { get; set; }    

        public string? Description { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0, 10, ErrorMessage = "Minimum is 0 and maximum 10")]
        public float Rating { get; set; }

        public int ReviewId { get; set; }
        public Review Review { get; set; }

        public ReviewItem() { }

        public ReviewItem(int carId, string? description, float rating, Review review)
        {
            CarId = carId;
            Description = description;
            Rating = rating;
            Review = review;
        }
    }
}
