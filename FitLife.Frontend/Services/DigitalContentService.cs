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

	public async Task<List<WorkoutProgram>> GetWorkoutProgramsAsync()
	{
		try
		{
			//Trying to fetch 
			return await _httpClient.GetFromJsonAsync<List<WorkoutProgram>>("getall");

		}
		catch (Exception ex)
		{

			throw ex;
		}
	}


}