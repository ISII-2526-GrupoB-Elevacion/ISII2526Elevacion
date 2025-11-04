using AppForSEII2526.API.DTOs.RentalDTO;

namespace AppForSEII2526.API.DTOs.ReviewDTO
{
    public class ReviewForCreateDTO
    {
        [StringLength(20, ErrorMessage = "Name cannot be any longer than 20 characters, neither shorter than 2.", MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "Surname cannot be any longer than 100 characters, neither shorter than 4.", MinimumLength = 4)]
        public string Surname { get; set; }

        public string UserName { get; set; }

        [StringLength(30, ErrorMessage = "Country cannot be any longer than 30 characters, neither shorter than 3.", MinimumLength = 3)]
        public string Country { get; set; }

        [StringLength(30, ErrorMessage = "DriverType cannot be any longer than 30 characters, neither shorter than 3.", MinimumLength = 3)]
        public string DriverType { get; set; }

        public IList<ReviewItemDTO> ReviewItems { get; set; }


        public ReviewForCreateDTO(string name, string surname, string userName, string country, string driverType, IList<ReviewItemDTO> reviewItems)
        {
            Name = name ?? throw new ArgumentNullException(nameof(Name));
            Surname = surname;
            UserName = userName ?? throw new ArgumentNullException(nameof(UserName));
            Country = country ?? throw new ArgumentNullException(nameof(Country));
            DriverType = driverType ?? throw new ArgumentNullException(nameof(DriverType));
            ReviewItems = reviewItems ?? throw new ArgumentNullException(nameof(ReviewItems));
        }

        public override bool Equals(object? obj)
        {
            return obj is ReviewForCreateDTO dTO &&
                   Name == dTO.Name &&
                   Surname == dTO.Surname &&
                   UserName == dTO.UserName &&
                   Country == dTO.Country &&
                   DriverType == dTO.DriverType &&
                   ReviewItems.SequenceEqual(dTO.ReviewItems);
        }
    }
}
