using System.Net.Http.Json;
using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services
{
    public class StatisticService
    {
        private readonly HttpClient _httpClient;

        public StatisticService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("StatisticService");
        }

        // Fetches all statistics
        public async Task<List<Statistic>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Statistic>>("Statistic")
                   ?? new List<Statistic>();
        }

        // Fetches a single statistic by id
        public async Task<Statistic?> GetByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<Statistic>($"Statistic/{id}");
        }

        // Fetches registered members for a statistic
        public async Task<List<Guid>> GetRegisteredAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<List<Guid>>($"Statistic/{id}/registered")
                   ?? new List<Guid>();
        }

        // Fetches attendance for a statistic
        public async Task<List<Guid>> GetAttendanceAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<List<Guid>>($"Statistic/{id}/attendance")
                   ?? new List<Guid>();
        }

        // Fetches waiting list for a statistic
        public async Task<List<Guid>> GetWaitingListAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<List<Guid>>($"Statistic/{id}/waitinglist")
                   ?? new List<Guid>();
        }
    }
}