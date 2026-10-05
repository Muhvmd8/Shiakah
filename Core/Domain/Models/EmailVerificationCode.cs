using Domain.Models.User;

namespace Domain.Models
{
    public class EmailVerificationCode : BaseEntity<int>
    {
        public string Code { get; set; } = default!;
        public DateTimeOffset ExpirationTime { get; set; }
        public string ApplicationUserId { get; set; } = default!;
        public ApplicationUser ApplicationUser { get; set; } = default!;
    }
}