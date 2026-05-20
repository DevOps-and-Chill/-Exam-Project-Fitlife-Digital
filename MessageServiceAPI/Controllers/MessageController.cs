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
    public async Task<IActionResult> SendMessage([FromBody] DirectMessage message)
    {
        await _messageService.SendDirectMessageAsync(message);
        return Ok();
    }
    
    // GET

    [HttpGet("{receiverId}/get-all-messages")]
    public async Task<IActionResult> GetAllMessages(Guid receiverId)
    { 
        await _messageService.GetAllMessagesAsync(receiverId);
        return Ok();
    }
    
    // PUT

    [HttpPut("{messageId}/mark-as-read")]
    public async Task<IActionResult> MarkAsRead(Guid messageId)
    {
        await  _messageService.MarkAsReadAsync(messageId);
        return Ok();
    }
    
    // DELETE
}