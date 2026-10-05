using Shared.Enums;

namespace Domain.Models.User
{
    public class UserProfileStyle : BaseEntity<int>
    {
        public int UserProfileId { get; set; }

        public Style Style { get; set; }

        public UserProfile UserProfile { get; set; } = null!;
    }
}