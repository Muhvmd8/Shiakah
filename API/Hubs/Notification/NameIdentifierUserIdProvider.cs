using Microsoft.AspNetCore.SignalR;

namespace API.Hubs.Notification
{
    public class NameIdentifierUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst("Id")?.Value
                ?? connection.User?.FindFirst("sub")?.Value
                ?? connection.UserIdentifier!;
        }
    }
}
