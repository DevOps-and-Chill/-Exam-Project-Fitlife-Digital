namespace MessageServiceAPI.Models;

public class ClassMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string PartitionKey { get; set; } = "inbox";
    public string ReceiverId { get; set; } = Guid.NewGuid().ToString();
    public string ClassId { get; set; } = Guid.NewGuid().ToString();
    public string Subject { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime TimeStart { get; set; }
    public DateTime TimeEnd { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}