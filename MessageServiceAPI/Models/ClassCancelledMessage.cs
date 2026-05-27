namespace MessageServiceAPI.Models;

public class ClassCancelledMessage : Message
{
    public string ClassId { get; set; } = Guid.NewGuid().ToString();
    public List<string> ReceiverIds { get; set; } = new();
    public string Subject { get; set; } = "";
    public DateTime TimeStart { get; set; }
    public DateTime TimeEnd { get; set; }
}