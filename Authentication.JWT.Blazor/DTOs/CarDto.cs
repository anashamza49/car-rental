using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace Authentication.JWT.Blazor.DTOs
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Brand { get; set; } 
        public string Model { get; set; } 
        public int Year { get; set; } 
        public string LicensePlate { get; set; }
        public string ImageUrl { get; set; }
    }
}
