namespace MessageServiceAPI.Models;

public class DirectMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string PartitionKey { get; set; } = "inbox";
    public string SenderId { get; set; } = Guid.NewGuid().ToString();
    public string ReceiverId { get; set; } = Guid.NewGuid().ToString();
    public string Subject { get; set; } = "";
    public string Content { get; set; } = "";
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
