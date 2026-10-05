using Shared.DTOs.Notification;

namespace ServiceAbstraction
{
    public interface INotificationService
    {
        Task SendAsync(NotificationCreateDto dto);
        //Task<PaginatedResultDto<NotificationDto>> GetUserNotificationsPagedAsync(
        //            string userId, int pageNumber, int pageSize);
        Task<int> GetUnreadCountAsync(string userId);
        Task MarkAsReadAsync(int id, string userId);
        Task MarkAllReadAsync(string userId);
        Task DeleteNotificationAsync(int id, string userId);
    }
}
