using System.Net.Http.Json;
using FitLife.Frontend.Models;
using FitLife.Frontend.Models.DTOs;

namespace FitLife.Frontend.Services;

public class TrainerService
{
    // Denne service håndterer personlige trænere og PT-funktionalitet.
    // Service-laget fungerer som bindeled mellem frontend og data.

    // HttpClient bruges til at kommunikere med UserService API
    private readonly HttpClient _httpClient;

    public TrainerService(IHttpClientFactory httpClientFactory)
    {
        // Opretter HttpClient med base URL fra Program.cs/appsettings.json
        _httpClient = httpClientFactory.CreateClient("UserService");
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