using System.Net.Http.Json;
using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class SessionService
{
    private readonly HttpClient _httpClient;
    private readonly TokenService _tokenService;

    public SessionService(IHttpClientFactory httpClientFactory, TokenService tokenService)
    {
        _httpClient = httpClientFactory.CreateClient("ClassService");
        _tokenService = tokenService;
        _tokenService.AttachToken(_httpClient);
    }

    // Henter alle hold fra backend
    public async Task<List<TrainingSession>> GetSessionsAsync()
    {
        //AO: Used for dev
        Console.WriteLine("SessionService.GetSessionsAsync() called from Oninit on Member");
        try
        {
            var sessions =
                await _httpClient.GetFromJsonAsync<List<TrainingSession>>("api/class");
            Console.WriteLine(sessions + "sessions fra db. Call made api/class (classservice)");

            return sessions ?? new List<TrainingSession>();
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"Kunne ikke hente hold fra ClassService. Fejl: {ex.Message}", ex);
        }
    }

    // Midlertidig frontend-logik
    public string Register(TrainingSession session)
    {
        session.IsUserSignedUp = true;
        session.RegisteredCount++;

        return $"Du er nu tilmeldt {session.Title}.";
    }

    public string Unregister(TrainingSession session)
    {
        session.IsUserSignedUp = false;
        session.RegisteredCount--;

        return $"Du er nu afmeldt {session.Title}.";
    }

    public string JoinWaitlist(TrainingSession session)
    {
        return $"Du er nu tilmeldt ventelisten til {session.Title}.";
    }
}