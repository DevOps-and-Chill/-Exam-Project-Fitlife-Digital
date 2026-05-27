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
    public async Task<List<Center>> GetCentersAsync()
    {
        try
        {
            var centers =
                await _httpClient.GetFromJsonAsync<List<Center>>(
                    "Facility/getall");

            return centers ?? new List<Center>();
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"Kunne ikke hente centre fra FacilityService. Fejl: {ex.Message}",
                ex);
        }
    }
}