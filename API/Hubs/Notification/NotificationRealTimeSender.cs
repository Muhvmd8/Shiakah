using Microsoft.AspNetCore.SignalR;
using ServiceAbstraction;

namespace API.Hubs.Notification
{
    public class NotificationRealTimeSender : INotificationRealTimeSender
    {
        private readonly IHubContext<NotificationHub> _hub;

        public NotificationRealTimeSender(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public async Task SendToUserAsync(string userId, object payload)
        {
            await _hub.Clients.User(userId)
                .SendAsync("ReceiveNotification", payload);
        }
    }
}
