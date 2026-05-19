namespace ClassServiceAPI.Messaging;

public class ClassCancelledMessage
{
    public Guid ClassId { get; set; }
    public string Title { get; set; } = "";
    public DateTime TimeStart { get; set; }
    public DateTime TimeEnd { get; set; }
    public List<Guid> MemberIds { get; set; } = new();
}