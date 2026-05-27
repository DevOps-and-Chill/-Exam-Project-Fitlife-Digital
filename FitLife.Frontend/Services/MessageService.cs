using System.Text.Json;
using FitLife.Frontend.Models;
using FitLife.Frontend.Models.DTOs;

namespace FitLife.Frontend.Services;

public class MessageService
{
    private readonly HttpClient _httpClient;
    
    public MessageService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("MessageService");
    }
    
    public async Task<List<MessageDto>> GetMessagesByReceiverAsync(string receiverId)
    {
        try
        {
            var messages = await _httpClient.GetFromJsonAsync<List<MessageDto>>(
                $"receivers/{receiverId}");
            return messages ?? new List<MessageDto>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not fetch message. Error: {ex.Message}", ex);
        }
    }
    public async Task SendDirectMessageAsync(DirectMessageDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("", dto);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not send a direct message. Error: {ex.Message}", ex);
        }
    }

    public async Task MarkAsReadAsync(string messageId)
    {
        try
        {
            var response = await _httpClient.PutAsync($"{messageId}/markasread", null);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not mark message as read. Error: {ex.Message}", ex);
        }
    }
    public async Task DeleteMessageAsync(string messageId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{messageId}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not delete message. Error: {ex.Message}", ex);
        }
    }
    
}

