
using System.ComponentModel.DataAnnotations;

namespace SIMS_SE06205.Models
{
    public class RegisterViewModel
    {
        [Key]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string? PhoneNumber { get; set; }

        //[Required(ErrorMessage = "Role is required.")]
        //public string Role { get; set; }
    }
}