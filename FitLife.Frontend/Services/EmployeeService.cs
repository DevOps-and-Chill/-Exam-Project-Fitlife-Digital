using System.Net.Http.Json;
using FitLife.Frontend.Models;
using FitLife.Frontend.Models.DTOs;


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
        var employee = await _httpClient.GetFromJsonAsync<Employee>($"employee/GetEmployeeById/{id}");
        return employee;
    }

    // Henter alle ansatte fra UserService og filtrerer kun dem der er personlige trænere
    // Kalder endpoint:
    // GET /employee/GetAllEmployees
    public async Task<List<Employee>> GetTrainersAsync()
    {
        try
        {
            var employees =
                await _httpClient.GetFromJsonAsync<List<Employee>>(
                    "employee/GetAllEmployees");
          
            return employees?
                .Where(employee => employee.IsPT && employee.ActiveUser)
                .Select(employee => new Employee
                {
                    
                    // Id kommer fra UserService
                    // Bruges senere som PersonalTrainerId ved booking i PTService
                    Id = employee.Id,

                    // Samler fornavn og efternavn til visning i UI
                    fullName = $"{employee.GivenName} {employee.FamilyName}",

                    // Midlertidig visningstekst
                    // TODO:
                    // Senere kan dette komme fra UserService, hvis Employee får profiltekst/speciale
                    Description = "Personlig træner hos FitLife.",
                    
                })
                .ToList()
                ?? new List<Employee>();
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"Kunne ikke hente trænere fra UserService. Fejl: {ex.Message}",
                ex);
        }
    }

    // Midlertidig ventelistefunktion
    // Bruges når en træner ikke er ledig.
    // TODO:
    // Skal senere gemmes i backend/database.
    public string JoinWaitlist(Employee employee)
    {
        return $"Du er nu tilmeldt ventelisten hos {employee.fullName}.";
    }

    // Midlertidig profilfunktion
    // TODO:
    // Senere kan denne metode hente detaljeret trænerprofil fra backend.
    public string ShowProfile(Employee employee)
    {
        return $"Profil for {employee.fullName} vises ikke som separat side endnu.";
    }
}