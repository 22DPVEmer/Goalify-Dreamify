using System.ComponentModel.DataAnnotations;

namespace Backend_Goalify.Core.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
} 