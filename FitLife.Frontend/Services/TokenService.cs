using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;

public class TokenService
{
    private readonly IMemoryCache _cache;

    public TokenService(IMemoryCache cache)
    {
        _cache = cache;
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
    public string? GetUserIdFromCachedToken()
    {
        var handler =
            new JwtSecurityTokenHandler();

        var jwt =
            handler.ReadJwtToken(GetToken());

        return jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

    }
}