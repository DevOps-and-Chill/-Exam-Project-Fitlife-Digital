
using MessageServiceAPI.Models;

namespace MessageServiceAPI.Services.Interfaces;
    public interface IMessageService
    {
        Task SendDirectMessageAsync(DirectMessage message);
        Task SendClassCancellationMessageAsync(ClassMessage message);
        
        Task<List<object>> GetAllMessagesAsync(Guid receiverId);
        Task MarkAsReadAsync(Guid messageId);
        
    }