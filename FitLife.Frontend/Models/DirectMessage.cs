namespace FitLife.Frontend.Models;

public class DirectMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string PartitionKey { get; set; } = "inbox";
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string Topic { get; set; } = "";
    public string Content { get; set; } = "";
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}