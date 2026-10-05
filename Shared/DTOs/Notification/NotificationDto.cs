using Shared.Enums;

namespace Shared.DTOs.Notification
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Message { get; set; } = default!;
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; }
        public string? DataJson { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
