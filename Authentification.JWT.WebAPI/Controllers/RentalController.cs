using Authentication.Shared.DTOs;
using Authentification.JWT.Service.Interfaces;
using Authentification.JWT.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Authentification.JWT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalController : ControllerBase
    {
        private readonly IRentalService rentalService;

        public RentalController(IRentalService rentalService)
        {
            this.rentalService = rentalService;
        }

        [Authorize]
        [HttpPost("rent")]
        public async Task<IActionResult> RentCar([FromBody] RentalDto rentalDto)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var rentalResponse = await rentalService.RentCarAsync(userId, rentalDto);

            return Ok(rentalResponse);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllRentals()
        {
            var rentals = await rentalService.GetAllRentalsAsync();
            return Ok(rentals);
        }
    }
}
