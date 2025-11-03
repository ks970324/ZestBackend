using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZestApi.Repositories;
using ZestApi.DTO;


namespace ZestApi.Services
{

    public class AuthService(IUserRepository userRepo, IConfiguration config) : IAuthService
    {
        public async Task<LoginResponse> LoginAsync(RequestLoginDto requestLoginDto)
        {
            var user = await userRepo.GetUserByEmailAsync(requestLoginDto.Email);

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(requestLoginDto.Password, user.Password_Hash);
            if (!isPasswordValid)
            {
                return new LoginResponse
                {
                    Status = false,
                    Token = null,
                };
            }

            user.Last_Login = DateTime.UtcNow;
            await userRepo.UpdateUserAsync(user);
            
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, requestLoginDto.Email)
            };

            var jwtKey = config["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenObj = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds,
                notBefore: DateTime.UtcNow
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenObj);

            return new LoginResponse
            {
                Status = true,
                Token = token,
            };
        }
        
        public async Task<GetCharactersResponse> GetCharacterAsync(string email)
        {
            var path = await userRepo.GetUserImageByEmailAsync(email);
            if (path == null)
            {
                return null;
            }

            return new GetCharactersResponse
            {
                CharactersPath = path
            };
        }
        
    }
}