namespace ZestApi.DTO;
using System.Text.Json.Serialization;

    public class RegisterRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("characterspath")]
        public string CharactersPath { get; set; }
    }

    public class RegisterDto
    {
        public string RegisterResult { get; set; }
        public Boolean Status { get; set; }
    }



    public class CheckEmailResponse
    {
        [JsonPropertyName("exists")]
        public bool Exists { get; set; }
    }

    public class CheckEmailRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
