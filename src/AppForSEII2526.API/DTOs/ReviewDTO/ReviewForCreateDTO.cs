namespace AppForSEII2526.API.DTOs.ReviewDTO
{
    public class ReviewForCreateDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Country { get; set; }
        public string DriverType { get; set; }
        public IList<ReviewItemDTO> ReviewItems { get; set; }


        public ReviewForCreateDTO(string name, string surname, string userName, string country, string driverType)
        {
            Name = name ?? throw new ArgumentNullException(nameof(Name));
            Surname = surname;
            UserName = userName ?? throw new ArgumentNullException(nameof(UserName));
            Country = country ?? throw new ArgumentNullException(nameof(Country));
            DriverType = driverType ?? throw new ArgumentNullException(nameof(DriverType));
        }
    }
}
