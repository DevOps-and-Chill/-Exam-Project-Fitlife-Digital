using System.Net.Http.Json;
using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class PTService
{
    private readonly HttpClient _httpClient;

    public PTService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("PTService");
    }

    public async Task<List<PersonalTrainingSession>> GetSessionsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<PersonalTrainingSession>>("PT")
               ?? new List<PersonalTrainingSession>();
    }

    public async Task<PersonalTrainingSession?> BookSessionAsync(PersonalTrainingSession session)
    {
        var response = await _httpClient.PostAsJsonAsync("PT", session);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<PersonalTrainingSession>();
    }

    public async Task<bool> CancelSessionAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"PT/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AcceptSessionAsync(Guid id)
    {
        var response = await _httpClient.PutAsync($"PT/{id}/accept", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RejectSessionAsync(Guid id)
    {
        var response = await _httpClient.PutAsync($"PT/{id}/reject", null);
        return response.IsSuccessStatusCode;
    }
}