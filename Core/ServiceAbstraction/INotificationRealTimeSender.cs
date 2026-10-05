namespace ServiceAbstraction
{
    public interface INotificationRealTimeSender
    {
        Task SendToUserAsync(string userId, object payload);
    }
}
