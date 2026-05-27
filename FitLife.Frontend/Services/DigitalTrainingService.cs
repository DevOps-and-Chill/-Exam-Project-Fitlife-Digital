using System.Net.Http;
using FitLife.Frontend.Models;
using FitLife.Frontend.Models.DigitalContent;

namespace FitLife.Frontend.Services;

public class DigitalTrainingService
{
    private readonly HttpClient _httpClient;
    private readonly TokenService _tokenService;

    public DigitalTrainingService(IHttpClientFactory httpClientFactory, TokenService tokenService)
    {
        // Opretter HttpClient med base URL fra Program.cs/appsettings.json.
        // Base URL peger på UserService.
        _httpClient = httpClientFactory.CreateClient("DigitalTrainingService");
        _tokenService = tokenService;
        _tokenService.AttachToken(_httpClient);
    }
    // Denne service håndterer digitalt træningsindhold.
    // Service-laget fungerer som bindeled mellem frontend og data.
    // TODO:
    // Senere skal data hentes fra backend/API i stedet for hardcoded mock data.

    // Midlertidig mock data til træningsvideoer
    // TODO:
    // Skal senere komme fra database/backend microservice.
    private readonly List<WorkoutVideo> _videos = new()
    {
        new WorkoutVideo
        {
            Name = "Squat teknik",
            Description = "Kort video om korrekt squat-teknik og typiske fejl.",
        },

        new WorkoutVideo
        {
            Name = "Core træning",
            Description = "Øvelser til mave, ryg og stabilitet.",
        },

        new WorkoutVideo
        {
            Name = "Opvarmning",
            Description = "En simpel opvarmningsrutine før træning.",
        }
    };

    // Midlertidig mock data til træningsprogrammer
    // TODO:
    // Skal senere komme fra database/backend microservice.
    private readonly List<WorkoutProgram> _programs = new()
    {
        new WorkoutProgram
        {
            Name = "Begynderprogram",
            Description = "Et simpelt program til nye medlemmer.",
        },

        new WorkoutProgram
        {
            Name = "Styrketræning 3 dage",
            Description = "Program med fokus på styrke og progression.",
        },

        new WorkoutProgram
        {
            Name = "Vægttab og kondition",
            Description = "Program med fokus på cardio og basis styrke.",
        }
    };

    // Returnerer alle træningsvideoer
    // Bruges af frontend til at vise digitalt videoindhold.
    // TODO:
    // Senere kan denne metode kalde et API-endpoint.
    public List<WorkoutVideo> GetVideos()
    {
        return _videos;
    }

    // Returnerer alle træningsprogrammer
    // Bruges af frontend til at vise træningsprogrammer.
    // TODO:
    // Senere kan denne metode kalde et API-endpoint.
    public List<WorkoutProgram> GetPrograms()
    {
        return _programs;
    }
}