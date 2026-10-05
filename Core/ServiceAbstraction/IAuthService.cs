using Shared.DTOs;
using Shared.DTOs.Auth;

namespace ServiceAbstraction
{
    public interface IAuthService
    {
        Task<Result<AuthResult>> LoginAsync(LoginRequest request);

        Task<Result<AuthResult>> ChangePasswordAsync(ChangePassRequest request);

        Task<Result> ForgetPasswordAsync(string email);

        Task<Result> VerifyResetPasswordOtpAsync(VerifyOtpRequest dto);

        Task<Result<AuthResult>> ResetPasswordAsync(ResetPasswordRequest dto);

        Task<Result> RegisterAsync(RegisterRequest dto);


        Task<Result<AuthResult>> VerifyRegistrationOtpAsync(VerifyOtpRequest dto);

        Task<Result<AuthResult>> RefreshTokenAsync(
            RefreshTokenRequest refreshTokenRequest);
    }
}
