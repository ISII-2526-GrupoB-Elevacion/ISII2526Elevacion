using AppForSEII2526.API.DTOs.PurchaseDTO;

namespace AppForSEII2526.API.DTOs.ReviewDTO
{
    public class ReviewDetailDTO
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Country { get; set; }

        public string DriverType { get; set; }

        public DateTime Created { get; set; }

        public IList<ReviewItemDTO> ReviewItems { get; set; }

        public ReviewDetailDTO(string name, string surname, string country, string driverType, DateTime created, IList<ReviewItemDTO> reviewItems )
        {
            Name = name;
            Surname = surname;
            Country = country;
            DriverType = driverType;
            Created = created;
            ReviewItems = reviewItems;
        }
    }
}
