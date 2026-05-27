
using MessageServiceAPI.Models;
using MessageServiceAPI.Models.DTOs;

namespace MessageServiceAPI.Services.Interfaces;
    public interface IMessageService
    { 
        Task SendDirectMessageAsync(DirectMessage message);
        Task SendClassCancellationMessageAsync(Message message);
        Task<List<MessageDto>> GetAllMessagesAsync(string receiverId);
        Task MarkAsReadAsync(string messageId);
        Task DeleteMessageAsync(string messageId);
    }