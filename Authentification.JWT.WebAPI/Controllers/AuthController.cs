using Authentification.JWT.Service.DTOs;
using Authentification.JWT.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Authentification.JWT.WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IUserService userService, IJwtService jwtService, ILogger<AuthController> logger) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            logger.LogInformation("attemp of register for {Username}", registerDto.Username);

            try
            {
                var userDto = await userService.RegisterUserAsync(
                    registerDto.Username,
                    registerDto.Email,
                    registerDto.Password);

                var token = jwtService.GenerateToken(userDto.Id);

                logger.LogInformation("registration working for {UserId}", userDto.Id);
                return Ok(new { User = userDto, Token = token });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "error to register {Username}", registerDto.Username);
                return BadRequest(new { Message = "Error to register ", Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            logger.LogInformation("attemp of login for {Username}", loginDto.Username);

            try
            {
                var user = await userService.GetUserByUsernameAsync(loginDto.Username);
                if (user == null)
                {
                    logger.LogWarning("User {Username} couldn't be found", loginDto.Username);
                    return Unauthorized(new { Message = "username unathorized" });
                }

                var isValid = await userService.VerifyPassword(user.Id, loginDto.Password);
                if (!isValid)
                {
                    logger.LogWarning("wrong password for {Username}", loginDto.Username);
                    return Unauthorized(new { Message = "user id or password incorrect" });
                }

                var token = jwtService.GenerateToken(user.Id);
                logger.LogInformation("login successed for {UserId}", user.Id);

                return Ok(new { User = user, Token = token });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "error login for {Username}", loginDto.Username);
                return StatusCode(500, new { Message = "intern error", Error = ex.Message });
            }
        }
    }
}