
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ZestApi.Controllers;



[ApiController]
[Route("api/[controller]")]
public class ZestAuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public ZestAuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    
    [HttpPost("Login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        try
        {

            if (request.Email == "test@example.com" && request.Password == "1234")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Email)
                };
                
                
                
                var JwtKey = _configuration["Jwt:Key"];

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var tokens = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(7),
                    signingCredentials: creds,
                    notBefore: DateTime.UtcNow
                );

                var token = new JwtSecurityTokenHandler().WriteToken(tokens);

                bool remember = request.Remember;

                return Ok(new LoginResponse()
                {
                    Token = token,
                    Status = true,
                    Remember = remember
                });
            }

            return Unauthorized();

        }
        catch (Exception ex)
        {
            Console.WriteLine("Login Error: " + ex.Message);
            return StatusCode(500, "Server Error: " + ex.Message);
        }
    }
}

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required bool Remember { get; set; }
    
}

public class LoginResponse
{
    public bool Status { get; set; }
    public string? Token { get; set; }
    public bool Remember { get; set; }
    
}
