using _DATN____SaleQQ.Dto.Request.AuthRequest;
using _DATN____SaleQQ.Dto.Response;
using _DATN____SaleQQ.Dto.Response.AuthResponse;
using _DATN____SaleQQ.Entity;

namespace _DATN____SaleQQ.Service
{
    public interface AuthService
    {
        Task<BaseResponse<LoginResponse>> Login(LoginRequest loginRequest);
        Task<BaseResponse<string>> Logout();



        Task<BaseResponse<ResetPasswordRequest>> ForgotPassword(string email);
        Task<BaseResponse<ResetPasswordRequest>> ChangePassword(ResetPasswordRequest resetPasswordRequest);



        Task<BaseResponse<ApplicationUser>> Register(RegisterRequest registerRequest);
        Task<BaseResponse<ConfirmEmailRequest>> ConfirmEmail(ConfirmEmailRequest confirmEmailRequest);
    }
}
