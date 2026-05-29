using FitLife.Frontend.Models;
using FitLife.Frontend.Models.DigitalContent;

namespace FitLife.Frontend.Services;

public class DigitalContentService
{
    private readonly HttpClient _httpClient;

    public DigitalContentService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("DigitalContentService");
    }

    // POST
    public async Task<WorkoutProgram> InsertWorkoutProgramAsync(WorkoutProgram workoutProgram)
    {
        var response = await _httpClient.PostAsJsonAsync("workoutprogram/insert", workoutProgram);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<WorkoutProgram>();
    }

    public async Task<WorkoutVideo> InsertWorkoutVideoAsync(WorkoutVideo workoutVideo)
    {
        var response = await _httpClient.PostAsJsonAsync("workoutvideo/insert", workoutVideo);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<WorkoutVideo>();
    }

    // GET
    public async Task<WorkoutProgram> GetWorkoutProgramAsync(string id)
    {
        return await _httpClient.GetFromJsonAsync<WorkoutProgram>($"workoutprogram/get/{id}");
    }

    public async Task<WorkoutVideo> GetWorkoutVideoAsync(string id)
    {
        return await _httpClient.GetFromJsonAsync<WorkoutVideo>($"workoutvideo/get/{id}");
    }

    public async Task<List<WorkoutProgram>> GetWorkoutProgramsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<WorkoutProgram>>("workoutprogram/getall") ?? new();
    }

    public async Task<List<WorkoutVideo>> GetWorkoutVideosAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<WorkoutVideo>>("workoutvideo/getall") ?? new();
    }

    // PUT
    public async Task<WorkoutProgram> UpdateWorkoutProgramAsync(string id, WorkoutProgram workoutProgram)
    {
        var response = await _httpClient.PutAsJsonAsync($"workoutprogram/update/{id}", workoutProgram);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<WorkoutProgram>();
    }

    public async Task<WorkoutVideo> UpdateWorkoutVideoAsync(string id, WorkoutVideo workoutVideo)
    {
        var response = await _httpClient.PutAsJsonAsync($"workoutvideo/update/{id}", workoutVideo);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<WorkoutVideo>();
    }

    // DELETE
    public async Task<WorkoutProgram?> DeleteWorkoutProgramAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"workoutprogram/delete/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<WorkoutProgram>();
    }

    public async Task<WorkoutVideo?> DeleteWorkoutVideoAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"workoutvideo/delete/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<WorkoutVideo>();
    }
}