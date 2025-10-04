namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(CarId), nameof(ReviewId))]
    public class ReviewItem
    {
        public int CarId { get; set; }
        public Car Car { get; set; }    

        [StringLength(100, ErrorMessage = "Description cannot be any longer than 100 characters, neither shorter than 15.", MinimumLength = 15)]
        public string Description { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0, 10, ErrorMessage = "Minimum is 0 and maximum 10")]
        public float Rating { get; set; }

        public int ReviewId { get; set; }
        public Review Review { get; set; }
    }
}
