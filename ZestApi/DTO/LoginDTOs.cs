using ZestApi.Controllers;

namespace ZestApi.DTO;
using System.Text.Json.Serialization;



    public class LoginRequest
    { 
        [JsonPropertyName("email")]   
        public required string Email { get; set; }
        [JsonPropertyName("password")]
        public required string Password { get; set; }

        public RequestLoginDto ExtractDto()
        {
            return new RequestLoginDto()
            {
                Email = Email,
                Password = Password
            };
        }
    }

    public class LoginResponse
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }
        [JsonPropertyName("token")]
        public string Token { get; set; }
    
    }

    public class GetCharactersResponse
    {
        [JsonPropertyName("characterspath")]
        public string CharactersPath { get; set; }
    }


