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
    
    public async Task<List<MessageDto>> GetAllMessagesAsync(Guid receiverId)
    {
        try
        {
            var messages = await _httpClient.GetFromJsonAsync<List<MessageDto>>(
                $"/message/get-all/{receiverId}");

            return messages ?? new List<MessageDto>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke hente messages. Fejl: {ex.Message}", ex);
        }
    }
    public async Task SendMessageAsync(DirectMessageDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/message/send", dto);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke sende besked. Fejl: {ex.Message}", ex);
        }
    }

    public async Task MarkAsReadAsync(Guid messageId)
    {
        try
        {
            var response = await _httpClient.PutAsync($"/message/mark-as-read/{messageId}", null);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke markere som læst. Fejl: {ex.Message}", ex);
        }
    }
    public async Task DeleteMessageAsync(Guid messageId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/message/delete/{messageId}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke markere som læst. Fejl: {ex.Message}", ex);
        }
    }
    
}

