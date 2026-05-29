namespace NotificationService.Models;

public class Message
{
    public Guid Id { get; set; }
    public Guid RecieverId { get; set; }
    public Guid SenderId { get; set; }
    public string Content { get; set; }
}