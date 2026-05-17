using NotificationService.Models;

namespace NotificationService.Repositories.Interfaces;

public interface INotificationRepository
{
    
    // POST
    Task SendNotificationAsync(Notification notification);
    
    // GET
    Task<List<Notification>> GetNotificationByRecieverAsync(string senderId);
    
}