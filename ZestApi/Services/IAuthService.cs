using ZestApi.DTO;

namespace ZestApi.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(RequestLoginDto requestLoginDto);
        Task<GetCharactersResponse> GetCharacterAsync(string email);
    }
}
