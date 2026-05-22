using System.Text.Json;
using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class MessageService
{
    private readonly HttpClient _httpClient;

    public MessageService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("MessageService");
    }
    
    public async Task<List<JsonElement>> GetAllMessagesAsync(Guid receiverId)
    {
        try
        {
            var messages = await _httpClient.GetFromJsonAsync<List<JsonElement>>(
                $"/api/message/{receiverId}/get-all-messages");

            return messages ?? new List<JsonElement>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke hente messages. Fejl: {ex.Message}", ex);
        }
    }
}

