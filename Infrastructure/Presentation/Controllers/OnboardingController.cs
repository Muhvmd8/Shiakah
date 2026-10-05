using ServiceAbstraction;
using Shared.DTOs;
using Shared.DTOs.OnBoarding;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OnboardingController(IOnboardingService onboardingService)
        : ApiController
    {
        [Authorize]
        [HttpPost("gender")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateGenderAsync([FromBody] UpdateGenderRequest request)
        {
            var result = await onboardingService.UpdateGenderAsync(GetIdFromToken(), request);
            return Ok(ApiResponse<object>.SuccessResponse(null!, result.Message));
        }

        [Authorize]
        [HttpPost("appearance")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateAppearanceAsync(
            [FromBody] UpdateAppearanceRequest request)
        {
            var result = await onboardingService.UpdateAppearanceAsync(GetIdFromToken(), request);
            return Ok(ApiResponse<object>.SuccessResponse(null!, result.Message));
        }

        [Authorize]
        [HttpPost("body")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateBodyAsync(
            [FromBody] UpdateBodyRequest request)
        {
            var result = await onboardingService.UpdateBodyAsync(GetIdFromToken(), request);
            return Ok(
                ApiResponse<object>.SuccessResponse(
                    null!,
                    result.Message));
        }

        [Authorize]
        [HttpPost("preferences")]
        public async Task<ActionResult<ApiResponse<object>>> UpdatePreferencesAsync(
            [FromBody] UpdatePreferencesRequest request)
        {
            var result =
                await onboardingService.UpdatePreferencesAsync(GetIdFromToken(), request);

            return Ok(ApiResponse<object>.SuccessResponse(null!, result.Message));
        }

        [Authorize]
        [HttpPost("profile")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateProfileAsync(
            [FromBody] UpdateProfileRequest request)
        {
            var result =
                await onboardingService.UpdateProfileAsync(GetIdFromToken(), request);

            return Ok(
                ApiResponse<object>.SuccessResponse(
                    null!,
                    result.Message));
        }

        [Authorize]
        [HttpGet("status")]
        public async Task<ActionResult<ApiResponse<OnboardingStatusResponse>>> GetOnboardingStatusAsync()
        {
            var result = await onboardingService.GetOnboardingStatusAsync(GetIdFromToken());

            return Ok(
                ApiResponse<OnboardingStatusResponse>.SuccessResponse(
                    result.Data!,
                    result.Message));
        }
    }
}
