
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
    
    public async Task CreateClassAsync(Class newClass)
    {
        var response = await _httpClient.PostAsJsonAsync("api/class", newClass);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<Class>> GetAllClassesAsync()
    {
        Console.WriteLine($"AUTH: {_httpClient.DefaultRequestHeaders.Authorization}");
        return await _httpClient.GetFromJsonAsync<List<Class>>("api/class/");
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