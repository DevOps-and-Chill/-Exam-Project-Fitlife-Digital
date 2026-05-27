using System.Net.Http.Json;
using FitLife.Frontend.Models;


namespace FitLife.Frontend.Services;

public class EmployeeService
{
    private readonly HttpClient _httpClient;
    private readonly TokenService _tokenService;

    //AO: Employees are registered and handled in userservice
    public EmployeeService(IHttpClientFactory httpClientFactory, TokenService tokenService)
    {
        _httpClient = httpClientFactory.CreateClient("UserService");
        _tokenService = tokenService;
        _tokenService.AttachToken(_httpClient);
    }

    public async Task<Employee> GetEmployeeById(string id)
    {
        _tokenService.AttachToken(_httpClient);
        var employee = await _httpClient.GetFromJsonAsync<Employee>($"employee/GetEmployeeById/{id}");
        return employee;
    }

    // Henter alle ansatte fra UserService og filtrerer kun dem der er personlige trænere
    // Kalder endpoint:
    // GET /employee/GetAllEmployees
    public async Task<List<Trainer>> GetTrainersAsync()
    {
        try
        {
            _tokenService.AttachToken(_httpClient);
            var employees = await _httpClient.GetFromJsonAsync<List<Employee>>("employee/GetAllEmployees");

            return employees?
                .Where(employee => employee.IsPT && employee.ActiveUser)
                .Select(employee => new Trainer
                {
                    // Id kommer fra UserService
                    // Bruges senere som PersonalTrainerId ved booking i PTService
                    Id = employee.Id,

                    // Samler fornavn og efternavn til visning i UI
                    Name = $"{employee.GivenName} {employee.FamilyName}",

                    // Midlertidig visningstekst
                    // TODO:
                    // Senere kan dette komme fra UserService, hvis Employee får profiltekst/speciale
                    Description = "Personlig træner hos FitLife.",

                    // Midlertidigt speciale
                    // TODO:
                    // Senere kan dette komme fra UserService eller PTService
                    Specialty = "Personlig træning",

                    // Midlertidig availability
                    // TODO:
                    // Senere kan dette beregnes ud fra bookingkalender/sessioner
                    IsAvailable = true
                })
                .ToList()
                ?? new List<Trainer>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke hente trænere fra UserService. Fejl: {ex.Message}",
                ex);
        }
    }

    // Midlertidig ventelistefunktion
    // Bruges når en træner ikke er ledig.
    // TODO:
    // Skal senere gemmes i backend/database.
    public string JoinWaitlist(Trainer trainer)
    {
        return $"Du er nu tilmeldt ventelisten hos {trainer.Name}.";
    }

    // Midlertidig profilfunktion
    // TODO:
    // Senere kan denne metode hente detaljeret trænerprofil fra backend.
    public string ShowProfile(Trainer trainer)
    {
        return $"Profil for {trainer.Name} vises ikke som separat side endnu.";
    }

    // Midlertidig beskedfunktion
    // TODO:
    // Senere skal denne kobles til MessageService/backend.
    public string SendMessage(Trainer trainer)
    {
        return $"Din besked til {trainer.Name} er sendt.";
    }
}