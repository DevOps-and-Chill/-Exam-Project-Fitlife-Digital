namespace FitLife.Frontend.Models.DTOs;

public class DirectMessageDto
{
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string Subject { get; set; } = "";
    public string Content { get; set; } = "";
}