using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using FitLife.Frontend.Services;

public class TokenService
{
    private readonly IMemoryCache _cache;
    private readonly HttpClient _httpClient;
    public TokenService(IMemoryCache cache, IHttpClientFactory httpClientFactory)
    {
        _cache = cache;
        _httpClient = httpClientFactory.CreateClient("UserService");
    }

    public void SetToken(string token)
    {
        var handler =
            new JwtSecurityTokenHandler();

        var jwt =
            handler.ReadJwtToken(token);

        var expires =
            jwt.ValidTo;

        var lifetime =
            expires - DateTime.UtcNow;

        _cache.Set("jwt", token, lifetime);

        //AO: Used during dev
        //var cachedToken =
        //    _cache.Get<string>("jwt");

        //Console.WriteLine(
        //    $"Cache virker: {cachedToken != null} + tokenstring: {cachedToken}");
    }

    public string? GetToken()
    {
        return _cache.Get<string>("jwt");
    }

    public void Clear()
    {
        _cache.Remove("jwt");
    }

    //AO: Pulls userId from token
    public Task<string?> GetUserIdFromCachedToken()
    {
        var handler = new JwtSecurityTokenHandler();

        var jwt = handler.ReadJwtToken(GetToken());

        string id = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        return Task.FromResult(id);
    }

    public async Task<string?> GetRoleBasedOnToken()
    {
        AttachToken(_httpClient);
        var userId = await GetUserIdFromCachedToken();
        var user = await _httpClient.GetFromJsonAsync<UserDto>($"user/GetUserById/{userId}");
        return user?.RoleName;
    }

    public void AttachToken(HttpClient client)
    {
        var token = GetToken();

        client.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrWhiteSpace(token))
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);
        }
    }

    public class UserDto
    {
        public string Id { get; set; }

        public string RoleName { get; set; }
    }


}