
using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class ClassService
{
    private readonly HttpClient _httpClient;

    public ClassService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ClassService");
    }
    
    public async Task CreateClassAsync(Class newClass)
    {
        var response = await _httpClient.PostAsJsonAsync("class", newClass);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<Class>> GetAllClassesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Class>>("class") ?? new();
    }

    public async Task RegisterMemberAsync(string classId, Member member)
    {
        var response = await _httpClient.PostAsJsonAsync($"class/{classId}/members", member);
        response.EnsureSuccessStatusCode();
    }

    public async Task UnregisterMemberAsync(string classId, string memberId)
    {
        var response = await _httpClient.DeleteAsync($"class/{classId}/members/{memberId}");
        response.EnsureSuccessStatusCode();
    }

    public async Task JoinWaitlistAsync(string classId, Member member)
    {
        var response = await _httpClient.PostAsJsonAsync($"class/{classId}/waitinglist", member);
        response.EnsureSuccessStatusCode();
    }
}