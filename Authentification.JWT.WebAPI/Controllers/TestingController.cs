using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentification.JWT.WebAPI.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestingController : ControllerBase
    {
        [HttpGet("secured")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult SecuredEndpoint()
        {
            return Ok("Authorized !!!");
        }
    }
}