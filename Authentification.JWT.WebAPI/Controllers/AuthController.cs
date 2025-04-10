using Authentification.JWT.Service.DTOs;
using Authentification.JWT.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Authentification.JWT.WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IUserService userService, IJwtService jwtService, ILogger<AuthController> logger) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            logger.LogInformation("Attempt to register {Username} with role {Role}",
                registerDto.Username, registerDto.Role);

            try
            {

                if (registerDto.Role != "Admin" && registerDto.Role != "Employee")
                {
                    logger.LogWarning("Invalid role {Role} for {Username}",
                        registerDto.Role, registerDto.Username);
                    return BadRequest(new { Message = "Role must be either 'Admin' or 'Employee'" });
                }

                var userDto = await userService.RegisterUserAsync(
                    registerDto.Username,
                    registerDto.Email,
                    registerDto.Password,
                    registerDto.Role);

                var token = jwtService.GenerateToken(userDto.Id, userDto.Role);

                logger.LogInformation("Registration successful for {UserId} with role {Role}",
                    userDto.Id, userDto.Role);

                return Ok(new
                {
                    User = userDto,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error registering {Username}", registerDto.Username);
                return BadRequest(new { Message = "Registration failed", Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            logger.LogInformation("Login attempt for {Username}", loginDto.Username);

            try
            {
                var user = await userService.GetUserByUsernameAsync(loginDto.Username);
                if (user == null)
                {
                    logger.LogWarning("User {Username} not found", loginDto.Username);
                    return Unauthorized(new { Message = "Invalid credentials" });
                }

                var isValid = await userService.VerifyPassword(user.Id, loginDto.Password);
                if (!isValid)
                {
                    logger.LogWarning("Invalid password for {Username}", loginDto.Username);
                    return Unauthorized(new { Message = "Invalid credentials" });
                }

                var token = jwtService.GenerateToken(user.Id, user.Role);

                logger.LogInformation("Login successful for {UserId} with role {Role}",
                    user.Id, user.Role);

                return Ok(new
                {
                    User = new
                    {
                        user.Id,
                        user.Username,
                        user.Email,
                        user.Role
                    },
                    Token = token
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during login for {Username}", loginDto.Username);
                return StatusCode(500, new { Message = "Login failed", Error = ex.Message });
            }
        }
    }
}