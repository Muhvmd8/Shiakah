using Domain.Models.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class RefreshToken : BaseEntity<int>
    {
        public string UserId { get; set; } = default!;
        public string Token { get; set; } = default!;
        public string JwtId { get; set; } = default!;
        public bool IsRevoked { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpiresAt { get; set; }
        [NotMapped]
        public bool IsActive => !IsUsed && !IsRevoked && ExpiresAt > DateTime.UtcNow;
        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; } = default!;
    }
}
