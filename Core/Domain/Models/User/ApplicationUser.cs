using Microsoft.AspNetCore.Identity;
using Shared.Enums;

namespace Domain.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsBanned { get; set; } = false;
        public ICollection<Notification> Notifications { get; set; } = default!;
        public EmailVerificationCode EmailVerificationCode { get; set; } = default!;
        public ICollection<RefreshToken> RefreshTokens { get; set; } = default!;
        public UserProfile? UserProfile { get; set; }
        public Wardrobe? Wardrobe { get; set; }
        public ICollection<OutfitResult> OutfitResults { get; set; } = [];
        public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public OnboardingStep OnboardingStep { get; set; } = OnboardingStep.Gender;
        public bool OnboardingCompleted { get; set; }
    }
}