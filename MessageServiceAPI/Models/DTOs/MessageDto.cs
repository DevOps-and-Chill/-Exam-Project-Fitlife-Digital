namespace MessageServiceAPI.Models.DTOs;

public class MessageDto
{
    public string Id { get; set; } = "";
    public string Subject { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public string Type { get; set; } = "";
}