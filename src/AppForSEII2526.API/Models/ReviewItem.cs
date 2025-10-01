namespace AppForSEII2526.API.Models
{
    public class ReviewItem
    {
        [Key]
        public int CarId { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; }
        [Key]
        public int ReviewId { get; set; }
    }
}
