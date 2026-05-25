using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class CurrentUserService
{
    private readonly TokenService _tokenService;
    public string? Token { get; private set; }

    public Member? CurrentUser { get; private set; }

    public bool IsLoggedIn => CurrentUser is not null && !string.IsNullOrWhiteSpace(Token);

    public string RoleName => CurrentUser?.RoleName ?? "";

    //AO: Mangler at implementere at hente den konkrete bruger fra userservcie
    public async Task SetCurrentUser()
    {
        var userId = _tokenService.GetUserIdFromCachedToken();

    }

    public void Logout()
    {
        Token = null;
        CurrentUser = null;
    }
}