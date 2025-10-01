
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Npgsql;
using ZestApi.Data;





namespace ZestApi.Controllers
{

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
        [AllowAnonymous]

        public IActionResult Login([FromBody] LoginRequest request, [FromServices] AppDbContext db)
        {
            try
            {
                var user = db.userinfo
                    .FirstOrDefault(u => u.email == request.Email && u.password_hash == request.Password);

                if (user != null)
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

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var email = User.Identity?.Name;
            if (email == null) return Unauthorized();

            var user = new { Email = email };
            return Ok(new { user });
        }
        
        
        [Authorize]
        [HttpGet("getcharacters")]
        public IActionResult GetCharacters()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            var connStr = _configuration.GetConnectionString("PostgresDb");
            using var conn = new NpgsqlConnection(connStr);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "SELECT image FROM userinfo WHERE email = @Email LIMIT 1", conn
            );
            cmd.Parameters.AddWithValue("Email", email);

            var result = cmd.ExecuteScalar();
            if (result == null || result == DBNull.Value)
            {
                return NotFound(new { Message = "No image found" });
            }

            return Ok(new GetCharactersResponse()
            {
                Characterspath = (string)result
            });
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


public class GetCharactersResponse
{
    public string Characterspath { get; set; }
}
