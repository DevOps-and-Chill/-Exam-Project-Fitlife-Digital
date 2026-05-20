using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class CurrentUserService
{
    public string? Token { get; private set; }

    public Member? CurrentUser { get; private set; }

    public bool IsLoggedIn => CurrentUser is not null && !string.IsNullOrWhiteSpace(Token);

    public string RoleName => CurrentUser?.RoleName ?? "";

    public void SetCurrentUser(string token, Member user)
    {
        Token = token;
        CurrentUser = user;
    }

    public void Logout()
    {
        Token = null;
        CurrentUser = null;
    }
}