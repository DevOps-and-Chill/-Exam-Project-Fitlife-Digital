namespace MessageServiceAPI.Models.DTOs;

public class SystemMessageDto : MessageDto
{
    public List<string> ReceiverIds { get; set; } = new();
}