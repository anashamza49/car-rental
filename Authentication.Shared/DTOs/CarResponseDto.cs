using System.ComponentModel.DataAnnotations;

namespace Authentification.JWT.Service.DTOs
{
    public class CarResponseDto
    {
        public int Id { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        [Range(1900, 2100)]
        public int Year { get; set; }

        [Required]
        public string LicensePlate { get; set; }

        public int OwnerId { get; set; }

        public byte[] Image { get; set; }
    }
}
