using System.ComponentModel.DataAnnotations;

namespace Authentification.JWT.Service.DTOs
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
