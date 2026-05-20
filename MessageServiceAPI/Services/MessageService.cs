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
    
    public async Task<List<object>> GetAllMessagesAsync(Guid receiverId)
    {
        var directMessages = await _context.DirectMessages
            .Where(m => m.ReceiverId == receiverId)
            .ToListAsync();

        var classMessages = await _context.ClassMessages
            .Where(m => m.ReceiverId == receiverId)
            .ToListAsync();

        return directMessages.Cast<object>()
            .Concat(classMessages.Cast<object>())
            .OrderByDescending(m => m is DirectMessage dm ? dm.CreatedAt : ((ClassMessage)m).CreatedAt)
            .ToList();
    }

    public async Task MarkAsReadAsync(Guid messageId)
    {
        var direct = await _context.DirectMessages.FindAsync(messageId);
        if (direct is not null)
        {
            direct.IsRead = true;
            await _context.SaveChangesAsync();
            return;
        }

        var classMsg = await _context.ClassMessages.FindAsync(messageId);
        if (classMsg is not null)
        {
            classMsg.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }
}