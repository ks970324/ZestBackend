using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace ZestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        
        private readonly IConfiguration _config;

        public RegisterController(IConfiguration config)
        {
            _config = config;
        }

        
        
        [HttpGet("checkemail")]
        public IActionResult CheckEmail([FromQuery] string email)
        {
            var connStr = _config.GetConnectionString("PostgresDb");

            using var conn = new NpgsqlConnection(connStr);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT EXISTS (SELECT 1 FROM userinfo WHERE email = @email)", conn);
            cmd.Parameters.AddWithValue("email", email);

            var exists = (bool)cmd.ExecuteScalar();

            return Ok(new checkEmailResponse { exists = exists });
        }

        
        [HttpPost("add")]
        public IActionResult AddUser([FromBody] RegisterDto registerDto)
        {
            try
            {
                var connStr = _config.GetConnectionString("PostgresDb");

                using var conn = new NpgsqlConnection(connStr);
                conn.Open();

                using var cmd =
                    new NpgsqlCommand(
                        "INSERT INTO userinfo (email,password_hash,image) VALUES (@email,@password_hash,@image)", conn);

                cmd.Parameters.AddWithValue("email", registerDto.Email);
                cmd.Parameters.AddWithValue("password_hash", registerDto.Password);
                cmd.Parameters.AddWithValue("image", registerDto.Characterspath);
                cmd.ExecuteNonQuery();


                return Ok(new { message = "User added!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error adding user: " + ex.Message });
            }
        }


    }




    public class RegisterDto
    {
        public string Email { get; set; }
        public int Password { get; set; }
        public string Characterspath { get; set; }
    }



    public class checkEmailResponse
    { 
        public bool exists { get; set; }
    }
    
    public class checkEmailRequest
    {
        public string Email { get; set; }
    }

}

