using Domain.Models.User;
using Shared.Enums;

namespace Domain.Models
{
    public class Notification : BaseEntity<int>
    {
        public string ApplicationUserId { get; set; } = default!;
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public NotificationType Type { get; set; }
        public string Message { get; set; } = default!;

        /// <summary>
        /// Optional JSON payload to store extra contextual data 
        /// (e.g. related VideoId, PlaylistId, FromUserId, etc.).
        /// </summary>
        public string? DataJson { get; set; }

        public bool IsRead { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}