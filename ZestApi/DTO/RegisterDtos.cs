namespace ZestApi.DTO;
using System.Text.Json.Serialization;

    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Characterspath { get; set; }
    }

    public class RegisterDto
    {
        public string RegisterResult { get; set; }
    }



    public class CheckEmailResponse
    {
        [JsonPropertyName("exists")]
        public bool Exists { get; set; }
    }

    public class CheckEmailRequest
    {
        public string Email { get; set; }
    }
