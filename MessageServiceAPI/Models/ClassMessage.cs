namespace MessageServiceAPI.Models;

public class ClassMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string PartitionKey { get; set; } = "inbox";
    public Guid ReceiverId { get; set; }
    public Guid ClassId { get; set; }
    public string Topic { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime TimeStart { get; set; }
    public DateTime TimeEnd { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}