using System.ComponentModel.DataAnnotations;

namespace AuthorizationMVC.Models.DTO
{
    public class RegistrationModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
       // [RegularExpression("^(?=.*?=.?[A-Z])(?[a-z](?=.*?[0-9](?=.*[#^$+=!*()@%&]).{6,}", ErrorMessage ="Xato")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        public string? Role { get; set; }
    }
}
