using Domain.Models;
using Shared.Enums;
using System.Text.Json;

namespace Service.Helpers
{
    public static class NotificationFactoryHelper
    {
        public static Notification Create(
            string userId,
            NotificationType type,
            string message,
            object? data = null)
        {
            return new Notification
            {
                ApplicationUserId = userId,
                Type = type,
                Message = message,
                DataJson = data is null ? null : JsonSerializer.Serialize(data),
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
