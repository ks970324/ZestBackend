using ZestApi.DTO;

namespace ZestApi.Services;

public interface IRegisterServices
{
    Task<RegisterDto> RegisterAsync(RegisterRequest request);
    Task<CheckEmailResponse> CheckEmailAsync(CheckEmailRequest request);
}