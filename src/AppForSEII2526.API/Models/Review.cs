namespace AppForSEII2526.API.Models
{
    public class Review
    {
        [StringLength(30, ErrorMessage = "Country cannot be any longer than 30 characters, neither shorter than 3.", MinimumLength = 3)]
        public string Country { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }

        [StringLength(30, ErrorMessage = "DriverType cannot be any longer than 30 characters, neither shorter than 3.", MinimumLength = 3)]
        public string DriverType { get; set; }

        [Key]
        public int Id { get; set; }

        public IList<ReviewItem> ReviewItems { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public Review() { }
      
        public Review(string country, DateTime created, string driverType, IList<ReviewItem> reviewItems, ApplicationUser applicationUser)
        {
            Country = country;
            Created = created;
            DriverType = driverType;
            ReviewItems = reviewItems;
            ApplicationUser = applicationUser;
        }
    }
}