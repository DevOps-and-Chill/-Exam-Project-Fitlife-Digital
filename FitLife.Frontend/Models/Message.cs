public class Message
{
    public int Id { get; set; }

    public string SenderName { get; set; } = "";
    public string ReceiverName { get; set; } = "";

    public string Topic { get; set; } = "";
    public string Content { get; set; } = "";

    public DateTime SentDate { get; set; }

    public bool IsRead { get; set; }
}