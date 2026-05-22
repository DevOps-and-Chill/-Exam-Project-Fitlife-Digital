using MessageServiceAPI.Models;
using MessageServiceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MessageServiceAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }
    // POST
    
    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessage([FromBody] MessageDto dto)
    {
        var message = new DirectMessage
        {
            SenderId = dto.SenderId,
            ReceiverId = dto.ReceiverId,
            Content = dto.Content,
            Subject = dto.Subject
        };

        await _messageService.SendDirectMessageAsync(message);

        return Ok("Message sent");
    }
    
    // GET

    [HttpGet("{receiverId}/get-all-messages")]
    public async Task<IActionResult> GetAllMessages(Guid receiverId)
    { 
        var messages = await _messageService.GetAllMessagesAsync(receiverId);
        return Ok(messages);
    }
    
    // PUT

    [HttpPut("{messageId}/mark-as-read")]
    public async Task<IActionResult> MarkAsRead(Guid messageId)
    {
        await  _messageService.MarkAsReadAsync(messageId);
        return Ok("Marked as read");
    }
    
    // DELETE
}