using System.ComponentModel.DataAnnotations;

namespace SIMS_SE06205.Models
{
    public class LoginViewModel
    {
        [Key]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Username can be not empty")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password can be not empty")]
        public string Password { get; set; }

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
    }
}
