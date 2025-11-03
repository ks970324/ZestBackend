
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using ZestApi.Services;
using ZestApi.DTO;





namespace ZestApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(ExceptionFilterAttribute))]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
        

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = await _authService.LoginAsync(request.ExtractDto());
                if (!result.Status)
                {
                    return Unauthorized(new { Message = "Invalid email or password." });
                }

                return Ok(result);
            }

            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error while user login");
                return StatusCode(500, new { message = ex.Message});
            }
                
                
        }

        [Authorize]
        [HttpGet("getcharacters")]
        public async Task<IActionResult>GetCharacters()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email)) return Unauthorized();

            var result = await _authService.GetCharacterAsync(email);
            if (result == null) return NotFound(new { Message = "No image found" });

            return Ok(result);
        }


    }

    public class ExceptionFilterAttribute  : IAsyncExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            throw new NotImplementedException();
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            throw new NotImplementedException();
        }
    }
}


