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
		try
		{
			return await _httpClient.GetFromJsonAsync<WorkoutProgram>("workoutprogram/insert");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
	
	public async Task<WorkoutVideo> InsertWorkoutVideoAsync(WorkoutVideo workoutVideo)
	{
		try
		{
			return await _httpClient.GetFromJsonAsync<WorkoutVideo>("workoutvideo/insert");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	
	
	
	// GET
	
	public async Task<WorkoutProgram> GetWorkoutProgramAsync(string id)
	{
		try
		{
			//Trying to fetch 
			return await _httpClient.GetFromJsonAsync<WorkoutProgram>("workoutprogram/get/{id}");
		}
		catch (Exception ex)
		{

			throw ex;
		}
	}
	
	public async Task<WorkoutVideo> GetWorkoutVideoAsync(string id)
	{
		try
		{
			//Trying to fetch 
			return await _httpClient.GetFromJsonAsync<WorkoutVideo>("workoutvideo/get/{id}");
		}
		catch (Exception ex)
		{

			throw ex;
		}
	}
	
	
	
	public async Task<List<WorkoutProgram>> GetWorkoutProgramsAsync()
	{
		try
		{
			//Trying to fetch 
			return await _httpClient.GetFromJsonAsync<List<WorkoutProgram>>("workoutprogram/getall");

		}
		catch (Exception ex)
		{

			throw ex;
		}
	}
	
	public async Task<List<WorkoutVideo>> GetWorkoutVideosAsync()
	{
		try
		{
			//Trying to fetch 
			return await _httpClient.GetFromJsonAsync<List<WorkoutVideo>>("workoutvideo/getall");

		}
		catch (Exception ex)
		{

			throw ex;
		}
	}

	// PUT
	
	public async Task<WorkoutProgram> UpdateWorkoutProgramAsync(string id, WorkoutProgram workoutProgram)
	{
		try
		{
			var response = await _httpClient.PutAsJsonAsync($"workoutprogram/update/{id}", workoutProgram);
			
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadFromJsonAsync<WorkoutProgram>();
		}
		catch (Exception ex)
		{

			throw ex;
		}
	}
	
	public async Task<WorkoutVideo> UpdateWorkoutVideoAsync(string id, WorkoutVideo workoutVideo)
	{
		try
		{
			
			var response = await _httpClient.PutAsJsonAsync($"workoutvideo/update/{id}", workoutVideo);
			
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadFromJsonAsync<WorkoutVideo>();
		}
		catch (Exception ex)
		{

			throw ex;
		}
	}
	
	// DELETE
	
	public async Task<WorkoutProgram?> DeleteWorkoutProgramAsync(string id)
	{
		try
		{
			var response = await _httpClient.DeleteAsync($"workoutprogram/delete/{id}");
			
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadFromJsonAsync<WorkoutProgram>();
		}
		catch (Exception)
		{
			throw;
		}
	}
	public async Task<WorkoutVideo?> DeleteWorkoutVideoAsync(string id)
	{
		try
		{
			var response = await _httpClient.DeleteAsync($"workoutvideo/delete/{id}");
			
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadFromJsonAsync<WorkoutVideo>();
		}
		catch (Exception)
		{
			throw;
		}
	}

}