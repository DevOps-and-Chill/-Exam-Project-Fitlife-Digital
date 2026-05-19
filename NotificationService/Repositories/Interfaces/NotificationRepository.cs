using NotificationService.Models;

namespace NotificationService.Repositories.Interfaces;

public class NotificationRepository : INotificationRepository
{
    
    // POST
    
    public async Task SendNotificationAsync(Notification notification)
    {
        
    }
    
    // GET
    public Task<List<Notification>> GetNotificationByRecieverAsync(string recieverId)
    {
        throw new NotImplementedException();
    }
    
}