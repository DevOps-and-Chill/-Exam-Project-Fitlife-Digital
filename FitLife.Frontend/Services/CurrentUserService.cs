using FitLife.Frontend.Models;
using Microsoft.Extensions.Caching.Memory;

namespace FitLife.Frontend.Services;

public class CurrentUserService
{
    private readonly TokenService _tokenService;
    private readonly MemberService _memberService;
    private readonly TrainerService _trainerService;

    private readonly IMemoryCache _cache;

    public CurrentUserService(IMemoryCache cache, TokenService tokenService)
    {
        _cache = cache;
        _tokenService = tokenService;
    }
    public string? Token { get; private set; }

    public User? CurrentUser { get; private set; }

    //public bool IsLoggedIn => _tokenService.G);

    public async Task SetCurrentUser()
    {
        CurrentUser ??= new User();

        var userId = await _tokenService.GetUserIdFromCachedToken();
        var role = await _tokenService.GetRoleBasedOnToken();

        if (role?.ToLower() == "member")
        {
            var member = await _memberService.GetMemberAsync(userId);

            if (member is not null)
            {
                CurrentUser.SetUserAsMember(member);

                _cache.Set("currentUser", member, TimeSpan.FromMinutes(60));
            }
        }

        else if (role?.ToLower() == "employee")
        {
            var employee = await _trainerService.GetEmployeeById(userId);

            if (employee is not null)
            {
                CurrentUser.SetUserAsEmployee(employee);

                _cache.Set("currentUser", employee, TimeSpan.FromMinutes(60));
            }
        }
    }

    public void Logout()
    {
        Token = null;
        CurrentUser = null;
    }
}