using System.Net.Http.Json;
using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class CenterService
{
    // HttpClient bruges til at kommunikere med FacilityService API
    private readonly HttpClient _httpClient;
    private readonly TokenService _tokenService;

    public CenterService(IHttpClientFactory httpClientFactory, TokenService tokenService)
    {
        // Opretter HttpClient med base URL fra Program.cs/appsettings.json
        _httpClient = httpClientFactory.CreateClient("FacilityService");
        _tokenService = tokenService;
        _tokenService.AttachToken(_httpClient);
    }

    // Henter alle centre fra FacilityService
    // Kalder endpoint:
    // GET /Facility/getall
    public async Task<List<ExerciseGym>> GetCentersAsync()
    {
        try
        {
            var centers = await _httpClient.GetFromJsonAsync<List<ExerciseGym>>("exercisegym/getall");

            return centers ?? new List<ExerciseGym>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke hente centre fra FacilityService. Fejl: {ex.Message}", ex);
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
	/// <summary>
	/// Method for updating exercisegyms
	/// </summary>
	/// <param name="exerciseGym"></param>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	public async Task UpdateExerciseGymAsync(ExerciseGym exerciseGym)
	{
		try
		{
			_tokenService.AttachToken(_httpClient);
			var response =
				await _httpClient.PutAsJsonAsync(
					"exercisegym/update",
					exerciseGym);

		}
		catch (Exception ex)
		{
			throw new Exception(
				$"Kunne ikke opdatere tr�ningscenter. Fejl: {ex.Message}",
				ex);
		}
	}

	/// <summary>
	/// method for updating a swimmingpool
	/// </summary>
	/// <param name="swimmingPool"></param>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	public async Task UpdateSwimmingPoolAsync(SwimmingPool swimmingPool)
	{
		try
		{
			_tokenService.AttachToken(_httpClient);
			var response =
				await _httpClient.PutAsJsonAsync(
					"swimmingpool/update",
					swimmingPool);

		}
		catch (Exception ex)
		{
			throw new Exception(
				$"Kunne ikke opdatere sv�mmehal. Fejl: {ex.Message}",
				ex);
		}
	}
}