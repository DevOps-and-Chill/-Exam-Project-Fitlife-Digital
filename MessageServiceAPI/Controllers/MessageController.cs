using MessageServiceAPI.Models;
using MessageServiceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MessageServiceAPI.Controllers;

[Route("message")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }
    // POST
    
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage(MessageDto dto)
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

    [HttpGet("get-all/{receiverId}")]
    public async Task<IActionResult> GetAllMessages(Guid receiverId)
    { 
        var messages = await _messageService.GetAllMessagesAsync(receiverId);
        return Ok(messages);
    }
    
    // PUT

    [HttpPut("mark-as-read/{messageId}")]
    public async Task<IActionResult> MarkAsRead(Guid messageId)
    {
        await  _messageService.MarkAsReadAsync(messageId);
        return Ok("Marked as read");
    }
    
    // DELETE
    
    [HttpDelete("delete/{messageId}")]
    public async Task<IActionResult> DeleteMessage(Guid messageId)
    {
        await  _messageService.DeleteMessageAsync(messageId);
        return Ok("Marked as read");
    }

}