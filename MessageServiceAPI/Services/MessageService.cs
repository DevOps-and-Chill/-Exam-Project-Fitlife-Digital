using MessageServiceAPI.Data;
using MessageServiceAPI.Models;
using MessageServiceAPI.Models.DTOs;
using MessageServiceAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MessageServiceAPI.Services;

public class MessageService : IMessageService
{
    private readonly MessageDbContext _context;

    public MessageService(MessageDbContext context)
    {
        _context = context;
    }
    
    public async Task SendDirectMessageAsync(DirectMessage message)
    {
        _context.DirectMessages.Add(message);
        await _context.SaveChangesAsync();
    }
    
    public async Task SendClassCancellationMessageAsync(Message message)
    {
        _context.SystemMessages.Add(message);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<MessageDto>> GetAllMessagesAsync(string receiverId)
    {
        var directMessages = await _context.DirectMessages
            .Where(m => m.ReceiverId == receiverId)
            .Select(m => new DirectMessageDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Subject = m.Subject,
                Content = m.Content,
                CreatedAt = m.CreatedAt,
                IsRead = m.IsRead,
                Type = "Direct"
            })
            .ToListAsync();

            var systemMessages = await _context.SystemMessages
                .Where(m => m.ReceiverIds.Contains(receiverId))
            .Select(m => new SystemMessageDto
            {
                Id = m.Id,
                ReceiverIds = m.ReceiverIds,
                Subject = m.Subject,
                Content = m.Content,
                CreatedAt = m.CreatedAt,
                IsRead = m.IsRead,
                Type = "System"
            })
            .ToListAsync();

            var result = new List<MessageDto>();
            result.AddRange(directMessages);
            result.AddRange(systemMessages);

            return result.OrderByDescending(m => m.CreatedAt).ToList();
    }

    public async Task MarkAsReadAsync(string messageId)
    {
        var directMessage = await _context.DirectMessages.FindAsync(messageId);
        if (directMessage is not null)
        {
            directMessage.IsRead = true;
            await _context.SaveChangesAsync();
            return;
        }

        var classMessage = await _context.SystemMessages.FindAsync(messageId);
        if (classMessage is not null)
        {
            classMessage.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteMessageAsync(string messageId)
    {
        var  directMessage = await _context.DirectMessages.FindAsync(messageId);
        if (directMessage is not null)
        {
            _context.DirectMessages.Remove(directMessage);
            await _context.SaveChangesAsync();
        }
    }
}