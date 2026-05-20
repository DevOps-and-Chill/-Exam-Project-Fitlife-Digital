namespace MessageServiceAPI.Models;

public class ClassCancelledMessage
{
    public Guid ClassId { get; set; }
    public List<Guid> ReceiverIds { get; set; } = new();
    public string Topic { get; set; } = "";
    public DateTime TimeStart { get; set; }
    public DateTime TimeEnd { get; set; }
}