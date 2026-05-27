namespace MessageServiceAPI.Models;

public class Message
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string PartitionKey { get; set; } = "inbox";
    public List<string> ReceiverIds { get; set; } = new();
    public string? ClassId { get; set; } = Guid.NewGuid().ToString();
    public string Subject { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime TimeStart { get; set; }
    public DateTime TimeEnd { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}