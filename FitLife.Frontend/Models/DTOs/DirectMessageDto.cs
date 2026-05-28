namespace FitLife.Frontend.Models.DTOs;

public class DirectMessageDto : MessageDto
{
    public string SenderId { get; set; } = "";
    public string ReceiverId { get; set; } = "";
}