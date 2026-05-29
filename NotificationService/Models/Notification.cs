namespace NotificationService.Models;

public class Notification
{
    public Guid Id { get; set; }
    public int? RecieverId { get; set; }
    public int? SenderId { get; set; }
    
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime TimeStamp { get; set; }
}