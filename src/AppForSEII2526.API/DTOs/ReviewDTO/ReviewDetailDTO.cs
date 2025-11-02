using AppForSEII2526.API.DTOs.PurchaseDTO;

namespace AppForSEII2526.API.DTOs.ReviewDTO
{
    public class ReviewDetailDTO
    {
        [StringLength(20, ErrorMessage = "Name cannot be any longer than 20 characters, neither shorter than 2.", MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "Surname cannot be any longer than 100 characters, neither shorter than 4.", MinimumLength = 4)]
        public string Surname { get; set; }

        [StringLength(30, ErrorMessage = "Country cannot be any longer than 30 characters, neither shorter than 3.", MinimumLength = 3)]
        public string Country { get; set; }

        [StringLength(30, ErrorMessage = "DriverType cannot be any longer than 30 characters, neither shorter than 3.", MinimumLength = 3)]
        public string DriverType { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
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

        public override bool Equals(object? obj)
        {
            return obj is ReviewDetailDTO dTO &&
                   Name == dTO.Name &&
                   Surname == dTO.Surname &&
                   Country == dTO.Country &&
                   DriverType == dTO.DriverType &&
                   Created == dTO.Created &&
                   ReviewItems.SequenceEqual(dTO.ReviewItems);
        }
    }
}
