namespace FitLife.Frontend.Models.DTOs;

public class DirectMessageDto
{
    public string SenderId { get; set; } = Guid.NewGuid().ToString();
    public string ReceiverId { get; set; } = Guid.NewGuid().ToString();
    public string Subject { get; set; } = "";
    public string Content { get; set; } = "";
}