using MessageServiceAPI.Data;
using MessageServiceAPI.Models;
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
    
    public async Task SendClassCancellationMessageAsync(ClassMessage message)
    {
        _context.ClassMessages.Add(message);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<MessageDto>> GetAllMessagesAsync(string receiverId)
    {
        var directMessages = await _context.DirectMessages
            .Where(m => m.ReceiverId == receiverId)
            .Select(m => new MessageDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.Content,
                CreatedAt = m.CreatedAt,
                Type = "direct"
            })
            .ToListAsync();

        var classMessages = await _context.ClassMessages
            .Where(m => m.ReceiverId == receiverId)
            .Select(m => new MessageDto
            {
                Id = m.Id,
                SenderId = m.ClassId,
                ReceiverId = m.ReceiverId,
                Content = m.Content,
                CreatedAt = m.CreatedAt,
                Type = "class"
            })
            .ToListAsync();

        return directMessages
            .Concat(classMessages)
            .OrderByDescending(m => m.CreatedAt)
            .ToList();
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

        var classMessage = await _context.ClassMessages.FindAsync(messageId);
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