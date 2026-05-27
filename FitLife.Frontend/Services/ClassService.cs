
using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class ClassService
{
    private readonly HttpClient _httpClient;
    private readonly TokenService _tokenService;


    public ClassService(IHttpClientFactory httpClientFactory, TokenService tokenService)
    {
        _httpClient = httpClientFactory.CreateClient("ClassService");
        _tokenService = tokenService;
        _tokenService.AttachToken(_httpClient);
    }
    
    // POST
    
    public async Task CreateClassAsync(Class newClass)
    {
        var response = await _httpClient.PostAsJsonAsync("api/class", newClass);
        response.EnsureSuccessStatusCode();
    }

    public async Task RegisterMemberToClassAsync(string classId, Member member)
    {
        var response = await _httpClient.PostAsJsonAsync($"{classId}/members", member);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task RegisterMemberToWaitinglistAsync(string classId, Member member)
    {
        var response = await _httpClient.PostAsJsonAsync($"{classId}/waitinglist", member);
        response.EnsureSuccessStatusCode();
    }
    
    // GET
    public async Task<List<Class>> GetAllClassesAsync()
    {
        Console.WriteLine($"AUTH: {_httpClient.DefaultRequestHeaders.Authorization}");
        return await _httpClient.GetFromJsonAsync<List<Class>>("api/class/");
    }
    
    public async Task<List<Class>> GetClassesByExerciseGymAsync(string exerciseGymId)
    {
        return await _httpClient.GetFromJsonAsync<List<Class>>($"exercisegyms/{exerciseGymId}") ?? new();
    }
    
    public async Task<Class> GetClassByIdAsync(string id)
    {
        return await _httpClient.GetFromJsonAsync<Class>($"{id}") ?? new();
    }
    
    public async Task<List<Class>> GetClassesByMemberAsync(string id)
    {
        return await _httpClient.GetFromJsonAsync<List<Class>>($"members/{id}") ?? new();
    }
    
    public async Task<List<Class>> GetClassesByEmployeeAsync(string id)
    {
        return await _httpClient.GetFromJsonAsync<List<Class>>($"employees/{id}") ?? new();
    }
    
    
    // PATCH
    
    public async Task CancelClassAsync(string classId)
    {
        var response = await _httpClient.PatchAsync(
            $"{classId}/cancel",
            null);

        response.EnsureSuccessStatusCode();
    }
    
    // DELETE
    
    public async Task DeleteClassAsync(string classId)
    {
        var response = await _httpClient.DeleteAsync(classId);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task UnregisterMemberFromClassAsync(string classId, string memberId)
    {
        var response = await _httpClient.DeleteAsync($"{classId}/members/{memberId}");
        response.EnsureSuccessStatusCode();
    }
    
    public async Task UnregisterMemberFromWaitinglistAsync(string classId, string memberId)
    {
        var response = await _httpClient.DeleteAsync(
            $"{classId}/waitinglist/{memberId}");

        response.EnsureSuccessStatusCode();
    }
}