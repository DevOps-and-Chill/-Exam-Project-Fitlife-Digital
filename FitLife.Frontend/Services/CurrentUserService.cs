using FitLife.Frontend.Models;
using Microsoft.Extensions.Caching.Memory;

namespace FitLife.Frontend.Services;

public class CurrentUserService
{
    private readonly TokenService _tokenService;
    private readonly MemberService _memberService;
    private readonly EmployeeServiceHjalte _employeeServiceHjalte;

    private readonly IMemoryCache _cache;

    public CurrentUserService(IMemoryCache cache, TokenService tokenService, MemberService memberService, EmployeeServiceHjalte employeeServiceHjalte)
    {
        _cache = cache;
        _tokenService = tokenService;
        _memberService = memberService;
        _employeeServiceHjalte = employeeServiceHjalte;
    }
    public User? CurrentUser { get; private set; }

    public async Task SetCurrentUser()
    {
        CurrentUser ??= new User();

        var userId = await _tokenService.GetUserIdFromCachedToken();

        if (string.IsNullOrWhiteSpace(userId))
        {
            return;
        }

        var role = await _tokenService.GetRoleBasedOnToken();

        if (string.IsNullOrWhiteSpace(role))
        {
            return;
        }

        if (role.Equals("member", StringComparison.OrdinalIgnoreCase))
        {
            var member = await _memberService.GetMemberAsync(userId);

            if (member is not null)
            {
                CurrentUser.SetUserAsMember(member);

                _cache.Set("currentUser", member, TimeSpan.FromMinutes(60));
            }
        }
        else if (role.Equals("employee", StringComparison.OrdinalIgnoreCase))
        {
            var employee = await _employeeServiceHjalte.GetEmployeeByIdAsync(userId);

            if (employee is not null)
            {
                CurrentUser.SetUserAsEmployee(employee);

                _cache.Set("currentUser", employee, TimeSpan.FromMinutes(60));
            }
        }
    }

    public void Logout()
    {
        _tokenService.Clear();
        CurrentUser = null;
    }
}