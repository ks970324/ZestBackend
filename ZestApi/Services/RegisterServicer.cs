using ZestApi.Repositories;
using ZestApi.Data;
using ZestApi.DTO;


namespace ZestApi.Services
{
    public class RegisterServices(IUserRepository userRepo) : IRegisterServices
    {
        public async Task<RegisterDto> RegisterAsync(RegisterRequest request)
        {
            bool exists = await userRepo.EmailExistsAsync(request.Email);
            if (exists)
            {
                return new RegisterDto { RegisterResult = "This email already exists!" };
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new UserInfo()
            {
                Email = request.Email,
                Password_Hash = hashedPassword,
                Image = request.CharactersPath,
                CreatedAt = DateTime.UtcNow,
                Is_Active = true,
                Last_Login = DateTime.UtcNow
            };

            await userRepo.AddUserAsync(user);

            return new RegisterDto
            {
                RegisterResult = "User added successfully!",
                Status = true,
            };
        }

        public async Task<CheckEmailResponse> CheckEmailAsync(CheckEmailRequest request)
        {
            bool exists = await userRepo.EmailExistsAsync(request.Email);
            return new CheckEmailResponse { Exists = exists };
        }
    }
}