using Authentification.JWT.Service.DTOs;
using Authentification.JWT.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Authentification.JWT.WebAPI.Controllers
{
    [Route("api/cars")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CarsController(ICarService carService, IWebHostEnvironment webHostEnvironment)
        {
            _carService = carService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Authorize(Policy = "EmployeeOrAdmin")]
        public async Task<IActionResult> GetUserCars()
        {
            var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var cars = await _carService.GetUserCarsAsync(ownerId);

            var carsWithImages = cars.Select(car => new
            {
                car.Id,
                car.Brand,
                car.Model,
                car.Year,
                car.LicensePlate,
                Image = car.Image != null ? Convert.ToBase64String(car.Image) : null
            }).ToList();

            return Ok(carsWithImages);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AddCar([FromForm] string brand, [FromForm] string model, [FromForm] int year,
            [FromForm] string licensePlate, [FromForm] string imageUrl)
        {
            try
            {
                var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var carDto = new CarDto
                {
                    Brand = brand,
                    Model = model,
                    Year = year,
                    LicensePlate = licensePlate,
                    ImageUrl = imageUrl
                };

                var carResponse = await _carService.AddCarAsync(ownerId, carDto);

                return Ok(carResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error uploading image", error = ex.Message });
            }
        }



        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var success = await _carService.DeleteCarAsync(id, ownerId);
            return success ? NoContent() : NotFound();
        }
    }
}
