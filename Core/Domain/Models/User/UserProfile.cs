using Shared.Enums;
namespace Domain.Models.User
{
    public class UserProfile : BaseEntity<int>
    {
        public string ApplicationUserId { get; set; } = default!;
        public ApplicationUser ApplicationUser { get; set; } = default!;

        public ICollection<UserProfileStyle> Styles { get; set; } = [];

        public ICollection<UserProfilePreferredColor> PreferredColors { get; set; } = [];
        public Gender Gender { get; set; } = Gender.Unknown;
        public AgeRange? AgeRange { get; set; }

        public SkinTone? SkinTone { get; set; }

        public decimal? Height { get; set; }

        public decimal? Weight { get; set; }

        public BodyType? BodyType { get; set; }

        public string? Name { get; set; }

        public string? Location { get; set; }
    }
}