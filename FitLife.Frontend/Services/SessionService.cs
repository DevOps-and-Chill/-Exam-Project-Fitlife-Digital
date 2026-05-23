using System.Net.Http.Json;
using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class SessionService
{
    private readonly HttpClient _httpClient;

    public SessionService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ClassService");
    }

    // Henter alle hold fra backend
    public async Task<List<TrainingSession>> GetSessionsAsync()
    {
        try
        {
            var sessions =
                await _httpClient.GetFromJsonAsync<List<TrainingSession>>(
                    "api/class");

            return sessions ?? new List<TrainingSession>();
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"Kunne ikke hente hold fra ClassService. Fejl: {ex.Message}",
                ex);
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