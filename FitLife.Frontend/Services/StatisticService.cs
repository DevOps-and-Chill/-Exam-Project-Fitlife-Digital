using System.Net.Http.Json;
using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services
{
    public class StatisticService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;

        public StatisticService(IHttpClientFactory httpClientFactory, TokenService tokenService)
        {
            _httpClient = httpClientFactory.CreateClient("StatisticService");
            _tokenService = tokenService;
            _tokenService.AttachToken(_httpClient);
        }

        // Fetches all statistics
        public async Task<List<Statistic>> GetAllAsync()
        {
            _tokenService.AttachToken(_httpClient);
            return await _httpClient.GetFromJsonAsync<List<Statistic>>("Statistic")
                   ?? new List<Statistic>();
        }

        // Fetches a single statistic by id
        public async Task<Statistic?> GetByIdAsync(Guid id)
        {
            _tokenService.AttachToken(_httpClient);
            return await _httpClient.GetFromJsonAsync<Statistic>($"Statistic/{id}");
        }

        // Fetches registered members for a statistic
        public async Task<List<Guid>> GetRegisteredAsync(Guid id)
        {
            _tokenService.AttachToken(_httpClient);
            return await _httpClient.GetFromJsonAsync<List<Guid>>($"Statistic/{id}/registered")
                   ?? new List<Guid>();
        }

        // Fetches attendance for a statistic
        public async Task<List<Guid>> GetAttendanceAsync(Guid id)
        {
            _tokenService.AttachToken(_httpClient);
            return await _httpClient.GetFromJsonAsync<List<Guid>>($"Statistic/{id}/attendance")
                   ?? new List<Guid>();
        }

        // Fetches waiting list for a statistic
        public async Task<List<Guid>> GetWaitingListAsync(Guid id)
        {
            _tokenService.AttachToken(_httpClient);
            return await _httpClient.GetFromJsonAsync<List<Guid>>($"Statistic/{id}/waitinglist")
                   ?? new List<Guid>();
        }

        // Creates a new statistic
        public async Task<Statistic?> CreateAsync(Statistic statistic)
        {
            _tokenService.AttachToken(_httpClient);
            var response = await _httpClient.PostAsJsonAsync("Statistic", statistic);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<Statistic>();
        }
    }
}