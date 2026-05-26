using System.Net.Http.Json;
using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class CenterService
{
    // HttpClient bruges til at kommunikere med FacilityService API
    private readonly HttpClient _httpClient;

    public CenterService(IHttpClientFactory httpClientFactory)
    {
        // Opretter HttpClient med base URL fra Program.cs/appsettings.json
        _httpClient = httpClientFactory.CreateClient("FacilityService");
    }

    // Henter alle centre fra FacilityService
    // Kalder endpoint:
    // GET /Facility/getall
    public async Task<List<ExerciseGym>> GetCentersAsync()
    {
        try
        {
            var centers =
                await _httpClient.GetFromJsonAsync<List<ExerciseGym>>(
                    "exercisegym/getall");

            return centers ?? new List<ExerciseGym>();
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"Kunne ikke hente centre fra FacilityService. Fejl: {ex.Message}",
                ex);
        }
    }
    /// <summary>
    /// Method for fetching all the swimming pools 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<List<SwimmingPool>> GetSwimmingPoolsAsync()
    {
		try
		{
			var swimmingPools =
				await _httpClient.GetFromJsonAsync<List<SwimmingPool>>(
					"swimmingpool/getall");

			return swimmingPools ?? new List<SwimmingPool>();
		}
		catch (Exception ex)
		{
			throw new Exception(
				$"Kunne ikke hente swimming pools fra FacilityService. Fejl: {ex.Message}",
				ex);
		}
	}
}