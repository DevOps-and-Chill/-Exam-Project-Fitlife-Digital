using NotificationService.Models;

namespace NotificationService.Repositories.Interfaces;

public class NotificationRepository : INotificationRepository
{
    public Task SendNotificationAsync(Notification notification)
    {
        throw new NotImplementedException();
    }
    
    public Task<List<Notification>> GetNotificationByRecieverAsync(string senderId)
    {
        throw new NotImplementedException();
    }
}