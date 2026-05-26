using MessageServiceAPI.Models;
using MessageServiceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MessageServiceAPI.Controllers;

[Route("message")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly ILogger<MessageController> _logger;

    public MessageController(IMessageService messageService, ILogger<MessageController> logger)
    {
        _messageService = messageService;
        _logger = logger;
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
        _logger.LogDebug("Sending message from {senderId} to {receiverId}", message.SenderId, message.ReceiverId);
        try
        {
            await _messageService.SendDirectMessageAsync(message);
            _logger.LogInformation("Message sent successfully");
            return Ok("Message sent");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error sending message: {message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }
    
    // GET

    [HttpGet("get-all/{receiverId}")]
    public async Task<IActionResult> GetAllMessages(Guid receiverId)
    { 
        _logger.LogDebug("Fetching all messages for receiver {receiverId}", receiverId);
        try
        {
            var messages = await _messageService.GetAllMessagesAsync(receiverId);
            return Ok(messages);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching messages for {receiverId}: {message}", receiverId, ex.Message);
            return BadRequest(ex.Message);
        }
    }
    
    // PUT

    [HttpPut("mark-as-read/{messageId}")]
    public async Task<IActionResult> MarkAsRead(Guid messageId)
    {
        _logger.LogDebug("Marking message {messageId} as read", messageId);
        try
        {
            await _messageService.MarkAsReadAsync(messageId);
            _logger.LogInformation("Message {messageId} marked as read", messageId);
            return Ok("Marked as read");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error marking message {messageId} as read: {message}", messageId, ex.Message);
            return BadRequest(ex.Message);
        }
    }
    
    // DELETE
    
    [HttpDelete("delete/{messageId}")]
    public async Task<IActionResult> DeleteMessage(Guid messageId)
    {
        await  _messageService.DeleteMessageAsync(messageId);
        return Ok("Marked as read");
    }

}