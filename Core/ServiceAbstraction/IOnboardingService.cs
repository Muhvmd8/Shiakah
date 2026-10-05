using Shared.DTOs;
using Shared.DTOs.OnBoarding;

namespace ServiceAbstraction
{
    public interface IOnboardingService
    {
        Task<Result> UpdateGenderAsync(string userId, UpdateGenderRequest request);
        Task<Result> UpdateAppearanceAsync(string userId, UpdateAppearanceRequest request);

        Task<Result> UpdateBodyAsync(string userId, UpdateBodyRequest request);

        Task<Result> UpdatePreferencesAsync(string userId, UpdatePreferencesRequest request);

        Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);

        Task<Result<OnboardingStatusResponse>> GetOnboardingStatusAsync(string userId);
    }
}
