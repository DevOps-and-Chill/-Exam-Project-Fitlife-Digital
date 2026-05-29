using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using FitLife.Frontend.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace FitLife.Frontend.Services;

public class AuthService
{
    private readonly HttpClient _authClient;
    private readonly HttpClient _userClient;
    private readonly TokenService _tokenService;
    private readonly CurrentUserService _currentUserService;

    public AuthService(IHttpClientFactory httpClientFactory, TokenService tokenService, CurrentUserService currentUserService)
    {
        _authClient = httpClientFactory.CreateClient("AuthService");
        _userClient = httpClientFactory.CreateClient("UserService");
        _tokenService = tokenService;
        _currentUserService = currentUserService;
    }

    public async Task<LoginResult> LoginAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return new LoginResult(false, null, null, "Indtast både email og password.");
        }

        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = password
        };

        try
        {
            var response = await _authClient.PostAsJsonAsync( "auth/Login", loginRequest);

            if (!response.IsSuccessStatusCode)
            {
                return new LoginResult(false, null, null, "Email eller password er forkert.");
            }

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (string.IsNullOrWhiteSpace(loginResponse?.Token))
            {
                return new LoginResult(false, null, null, "Login lykkedes ikke. Token mangler.");
            }
            _tokenService.SetToken(loginResponse.Token);

            string ?role = await _tokenService.GetRoleBasedOnToken();

            if (string.IsNullOrWhiteSpace(role))
            {
                return new LoginResult(false, loginResponse.Token, null, "Token mangler rolle.");
            }

            _currentUserService.SetAuthorization(true);
            return new LoginResult(true, loginResponse.Token, role, "Login lykkedes.");
        }
        catch (Exception ex)
        {
            return new LoginResult(false, null, null, $"Login fejlede. Fejl: {ex.Message}");
        }
    }

}

public class LoginRequest
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}

public class LoginResponse
{
    public string Token { get; set; } = "";
}

public record LoginResult(
    bool Success,
    string? Token,
    string? Role,
    string Message);