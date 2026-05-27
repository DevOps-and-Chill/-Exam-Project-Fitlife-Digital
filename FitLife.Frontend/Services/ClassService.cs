
using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class ClassService
{
    private readonly HttpClient _httpClient;

    public ClassService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ClassService");
    }
    
    // POST
    
    public async Task CreateClassAsync(Class newClass)
    {
        var response = await _httpClient.PostAsJsonAsync("", newClass);
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
        return await _httpClient.GetFromJsonAsync<List<Class>>("") ?? new();
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