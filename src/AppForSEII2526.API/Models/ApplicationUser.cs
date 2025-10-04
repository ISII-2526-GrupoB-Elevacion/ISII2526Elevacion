using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {
    [StringLength(20, ErrorMessage = "Name cannot be any longer than 20 characters, neither shorter than 2.", MinimumLength = 2)]
    public string Name { get; set; }

    [StringLength(100, ErrorMessage = "Surname cannot be any longer than 100 characters, neither shorter than 4.", MinimumLength = 4)]
    public string Surname { get; set; }

    [Key]
    [StringLength(30, ErrorMessage = "UserName cannot be any longer than 30 characters, neither shorter than 9.", MinimumLength = 9)]
    public string UserName { get; set; }
}