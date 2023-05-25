using Microsoft.AspNetCore.Identity;

namespace AuthorizationMVC.Models.Domain
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
