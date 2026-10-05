using ServiceAbstraction;
using Shared.DTOs;
using Shared.DTOs.Auth;

namespace Presentation.Controllers;

public class AuthController(IAuthService authService) : ApiController
{
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResult>>> Login([FromBody] LoginRequest request)
    {
        var result = await authService.LoginAsync(request);
        return Ok(ApiResponse<AuthResult>.SuccessResponse(result.Data!, result.Message));
    }

    [HttpPost("register/")]
    public async Task<ActionResult<ApiResponse<object>>> RegisterAsync([FromForm] RegisterRequest dto)
    {
        var result = await authService.RegisterAsync(dto);
        return Ok(ApiResponse<object>.SuccessResponse(null!, result.Message));
    }

    [HttpPost("verify-registration-otp")]
    public async Task<ActionResult<ApiResponse<AuthResult>>> VerifyRegistrationOtpAsync([FromBody] VerifyOtpRequest dto)
    {
        var result = await authService.VerifyRegistrationOtpAsync(dto);
        return Ok(ApiResponse<AuthResult>.SuccessResponse(result.Data!, result.Message));
    }

    [HttpPost("change-password")]
    public async Task<ActionResult<ApiResponse<AuthResult>>> ChangePasswordAsync(ChangePassRequest request)
    {
        var result = await authService.ChangePasswordAsync(request);
        return Ok(ApiResponse<AuthResult>.SuccessResponse(result.Data!, result.Message));
    }

    [HttpPost("forget-password")]
    public async Task<ActionResult<ApiResponse<object>>> ForgetPasswordAsync([FromBody] ForgetPasswordRequest dto)
    {
        var result = await authService.ForgetPasswordAsync(dto.Email);
        return Ok(ApiResponse<object>.SuccessResponse(null!, result.Message));
    }

    [HttpPost("verify-reset-password-otp")]
    public async Task<ActionResult<ApiResponse<object>>> VerifyResetPasswordOtpAsync([FromBody] VerifyOtpRequest dto)
    {
        var result = await authService.VerifyResetPasswordOtpAsync(dto);
        return Ok(ApiResponse<object>.SuccessResponse(null!, result.Message));
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult<ApiResponse<AuthResult>>> ResetPasswordAsync([FromBody] ResetPasswordRequest dto)
    {
        var result = await authService.ResetPasswordAsync(dto);
        return Ok(ApiResponse<AuthResult>.SuccessResponse(result.Data!, result.Message));
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<ApiResponse<AuthResult>>> RefreshTokenAsync([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        var result = await authService.RefreshTokenAsync(refreshTokenRequest);
        return Ok(ApiResponse<AuthResult>.SuccessResponse(result.Data!, result.Message));
    }
}