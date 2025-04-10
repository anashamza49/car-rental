
using System.ComponentModel.DataAnnotations;

namespace Authentification.JWT.Service.DTOs
{
    public class RegisterDto
    {
        [Required, MinLength(3)]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }
        public string Role { get; set; } // Optionnel : par défaut "Employee"
    }
}
