using Shared.Enums;

namespace Shared.DTOs.OnBoarding
{
    public class OnboardingStatusResponse
    {
        public OnboardingStep CurrentStep { get; set; }

        public bool Completed { get; set; }

        public OnboardingProfileResponse? Profile { get; set; }
    }
}