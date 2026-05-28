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

    // GET
    
    public async Task<List<Employee>> GetAllEmployeesAsync()
    {
        try
        {
            _tokenService.AttachToken(_httpClient);
            var employees = await _httpClient.GetFromJsonAsync<List<Employee>>("employee/GetAllEmployees");
            return employees ?? new List<Employee>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke hente medarbejdere fra UserService. Fejl: {ex.Message}", ex);
        }
    }
    
    public async Task<Employee?> GetEmployeeByIdAsync(string userId)
    {
        try
        {
            _tokenService.AttachToken(_httpClient);
            return await _httpClient.GetFromJsonAsync<Employee>($"employee/GetEmployeeById/{userId}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke hente medarbejder med id {userId}. Fejl: {ex.Message}", ex);
        }
    }
    
    public async Task<List<Employee>> GetEmployeesByAffiliationAsync(string affiliationId)
    {
        try
        {
            _tokenService.AttachToken(_httpClient);
            var employees = await _httpClient.GetFromJsonAsync<List<Employee>>($"employee/GetEmployeeByAffiliation/{affiliationId}");
            return employees ?? new List<Employee>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke hente medarbejdere for center {affiliationId}. Fejl: {ex.Message}", ex);
        }
    }

    // POST
    
    public async Task<Employee> UpsertEmployeeAsync(Employee employee)
    {
        try
        {
            _tokenService.AttachToken(_httpClient);
            var response = await _httpClient.PostAsJsonAsync("employee/UpsertEmployee", employee);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"UserService returnerede fejl: {(int)response.StatusCode} {response.ReasonPhrase}. {errorMessage}");
            }

            var upsertedEmployee = await response.Content.ReadFromJsonAsync<Employee>();
            return upsertedEmployee ?? employee;
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke oprette/opdatere medarbejder via UserService. Fejl: {ex.Message}", ex);
        }
    }

    // PUT
    
    public async Task EndEmploymentAsync(string userId)
    {
        try
        {
            _tokenService.AttachToken(_httpClient);
            var response = await _httpClient.PutAsync($"employee/EndEmploymentForEmployee/{userId}", null);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"UserService returnerede fejl: {(int)response.StatusCode} {response.ReasonPhrase}. {errorMessage}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke afslutte ansættelse for medarbejder {userId}. Fejl: {ex.Message}", ex);
        }
    }
    
    public async Task SetEmployeeAsManagerAsync(string userId)
    {
        try
        {
            _tokenService.AttachToken(_httpClient);
            var response = await _httpClient.PutAsync($"employee/SetEmployeeAsManager/{userId}", null);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"UserService returnerede fejl: {(int)response.StatusCode} {response.ReasonPhrase}. {errorMessage}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke sætte medarbejder {userId} som manager. Fejl: {ex.Message}", ex);
        }
    }
    
    public async Task SetAccountAsInactiveAsync(string userId)
    {
        try
        {
            _tokenService.AttachToken(_httpClient);
            var response = await _httpClient.PutAsync($"employee/SetAccountAsInactive/{userId}", null);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"UserService returnerede fejl: {(int)response.StatusCode} {response.ReasonPhrase}. {errorMessage}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke deaktivere medarbejderkonto {userId}. Fejl: {ex.Message}", ex);
        }
    }

    // DELETE
    
    public async Task<Employee> DeleteEmployeeAsync(string userId)
    {
        try
        {
            _tokenService.AttachToken(_httpClient);
            var response = await _httpClient.DeleteAsync($"employee/DeleteEmployee/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"UserService returnerede fejl: {(int)response.StatusCode} {response.ReasonPhrase}. {errorMessage}");
            }

            var deletedEmployee = await response.Content.ReadFromJsonAsync<Employee>();
            return deletedEmployee ?? new();
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke slette medarbejder {userId}. Fejl: {ex.Message}", ex);
        }
    }
}