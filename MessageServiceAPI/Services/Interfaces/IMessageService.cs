
using MessageServiceAPI.Models;

namespace MessageServiceAPI.Services.Interfaces;
    public interface IMessageService
    { 
        Task SendDirectMessageAsync(DirectMessage message);
        Task SendClassCancellationMessageAsync(ClassMessage message);
        Task<List<MessageDto>> GetAllMessagesAsync(string receiverId);
        Task MarkAsReadAsync(string messageId);
        Task DeleteMessageAsync(string messageId);
    }