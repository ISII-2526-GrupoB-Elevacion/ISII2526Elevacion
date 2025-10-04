namespace AppForSEII2526.API.Models
{
    public class Model
    {
        [Key]
        public int Id { get; set; }
        [StringLength(20, ErrorMessage = "Name cannot be any longer than 20 characters, neither shorter than 1.", MinimumLength = 1)]
        public string Name { get; set; }
    }
}
