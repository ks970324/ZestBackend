using Microsoft.AspNetCore.Mvc;

namespace ZestBackend.Contoller;

public class ZestLoginController : ControllerBase
{

    [HttpPost] // 指定用 POST 才能回應以下
    public BaseResponse Login([FromBody] LoginRequest request)
    {
        // 模擬帳號密碼驗證邏輯
        if (request.email == "test@example.com" && request.password == "1234")
        {
            return new BaseResponse()
            {
                Status = LoginStatus.Success
            }; // 驗證成功
        }


        return new BaseResponse()
        {
            Status = LoginStatus.Failed
        }; // 驗證失敗
    }
    public class LoginRequest
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}

public enum LoginStatus
{
    Success,
    Failed 
}

public class BaseResponse 
{
    public LoginStatus Status { get; set; }
}