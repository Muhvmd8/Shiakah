using Shared.Enums;

namespace Shared.DTOs.Notification
{
    public class NotificationCreateDto
    {
        public string UserId { get; set; } = default!;
        public NotificationType Type { get; set; }
        public string Message { get; set; } = default!;
        public object? Data { get; set; }
    }
}
