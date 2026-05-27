namespace ClassServiceAPI.Messaging;

public class ClassCancelledMessage
{
    public string ClassId { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = "";
    public DateTime TimeStart { get; set; }
    public DateTime TimeEnd { get; set; }
    public List<string> MemberIds { get; set; } = new();
}