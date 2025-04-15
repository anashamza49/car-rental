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
            var cars = await _carService.GetUserCarsAsync();

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

        [HttpGet("{id}")]
        [Authorize(Policy = "EmployeeOrAdmin")]
        public async Task<IActionResult> GetCarById(int id)
        {
            var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var car = await _carService.GetCarByIdAsync(id, ownerId);
                return Ok(car);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = "Car not found", error = ex.Message });
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

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] CarDto carDto)
        {
            try
            {
                var ownerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(ownerIdClaim))
                {
                    return Unauthorized(new { message = "User is not authorized." });
                }

                var ownerId = int.Parse(ownerIdClaim);
                var success = await _carService.UpdateCarAsync(ownerId, id, carDto);

                if (success)
                {
                    return Ok("Car updated successfully.");
                }
                else
                {
                    return BadRequest("Failed to update car.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating car", error = ex.Message });
            }
        }

    }
}
