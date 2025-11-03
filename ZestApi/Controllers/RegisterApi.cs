using Microsoft.AspNetCore.Mvc;
using ZestApi.Services;
using ZestApi.DTO;

namespace ZestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterServices _registerServices;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(IRegisterServices registerServices, ILogger<RegisterController> logger)
        {
            _registerServices = registerServices;
            _logger = logger;
        }

        [HttpGet("CheckEmail")]
        public async Task<IActionResult> CheckEmail([FromQuery] CheckEmailRequest request)
        {
            var result = await _registerServices.CheckEmailAsync(request);
            return Ok(result);
        }


        [HttpPost("Add")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var result = await _registerServices.RegisterAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while registering user");
                return StatusCode(500, new { message = ex.Message });
            }


        }
        


    }
}

