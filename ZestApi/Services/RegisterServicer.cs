using ZestApi.Repositories;
using BCrypt.Net;
using ZestApi.Data;
using ZestApi.DTO;


namespace ZestApi.Services
{
        public interface IRegisterServices
        {
            Task<RegisterDto> RegisterAsync(RegisterRequest request);
            Task<CheckEmailResponse> CheckEmailAsync(CheckEmailRequest request);
        }
        
        public class RegisterServices : IRegisterServices
        {
            private readonly IUserRepository _userRepo;
            public RegisterServices(IUserRepository userRepo)
            {
                _userRepo = userRepo;
            }

            public async Task<RegisterDto> RegisterAsync(RegisterRequest request)
            {
                bool exists = await _userRepo.EmailExistsAsync(request.Email);
                if (exists)
                {
                    return new RegisterDto { RegisterResult = "This email already exists!" };
                }
                
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var user = new UserInfo()
                {
                    Email = request.Email,
                    Password_hash = hashedPassword,
                    Image = request.Characterspath
                };
                
                await _userRepo.AddUserAsync(user);
                
                return new RegisterDto { RegisterResult = "User added successfully!" };
            }
            
            public async Task<CheckEmailResponse> CheckEmailAsync(CheckEmailRequest request)
            {
                bool exists = await _userRepo.EmailExistsAsync(request.Email);
                return new CheckEmailResponse { Exists = exists };
            
            }
        }
        
        
}