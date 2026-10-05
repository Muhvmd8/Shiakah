using Shared.Enums;

namespace Domain.Models.User
{
    public class UserProfilePreferredColor : BaseEntity<int>
    {
        public int UserProfileId { get; set; }

        public PreferredColor Color { get; set; }

        public UserProfile UserProfile { get; set; } = null!;
    }
}
