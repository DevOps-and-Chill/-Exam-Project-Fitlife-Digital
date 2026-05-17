namespace PTServiceAPI.Models
{
    public class Message
    {
        public Message(Guid senderId, Guid receiverId, string content)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            Content = content;
        }

        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid SenderId { get; private set; }
        public Guid ReceiverId { get; private set; }
        public string Content { get; private set; }
        public DateTime SentAt { get; init; } = DateTime.UtcNow;
    }
}