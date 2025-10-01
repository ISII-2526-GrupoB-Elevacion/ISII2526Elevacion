namespace AppForSEII2526.API.Models
{
    public class Review
    {
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public string DriverType { get; set; }
        [Key]
        public int Id { get; set; }
    }
}
