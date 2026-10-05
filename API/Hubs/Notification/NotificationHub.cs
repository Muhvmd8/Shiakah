using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs.Notification
{
    [Authorize]
    public class NotificationHub:Hub
    {
    }
}
