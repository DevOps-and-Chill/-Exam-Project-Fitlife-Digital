using FitLife.Frontend.Models;
using Microsoft.Extensions.Caching.Memory;

namespace FitLife.Frontend.Services;

public class CurrentUserService
{
    private readonly TokenService _tokenService;
    private readonly MemberService _memberService;
    private readonly EmployeeService _employeeService;
    private readonly IMemoryCache _cache;

    public CurrentUserService(IMemoryCache cache, TokenService tokenService, MemberService memberService, EmployeeService trainerService)
    {
        _cache = cache;
        _tokenService = tokenService;
        _memberService = memberService;
        _employeeService = trainerService;
    }
    public User? CurrentUser { get; private set; }
    public string CurrentUserRole { get; private set; }
    public string CurrentUserMembership { get; private set; }
    public Boolean isAuthorized { get; private set; } = false;
    public Boolean isLoaded { get; private set; } = false;

    /// <summary>
    ///AO: Uses the cached token to check userid, get role for the user and based on role sets the CurrentUser in memorycache. 
    /// </summary>
    /// <returns></returns>
    public async Task SetCurrentUser()
    {
        CurrentUser ??= new User();
        CurrentUserRole = "unknown";

        var userId = await _tokenService.GetUserIdFromCachedToken();

        if (string.IsNullOrWhiteSpace(userId))
        {
            isLoaded = true;
            return;
        }

        var role = await _tokenService.GetRoleBasedOnToken();

        if (string.IsNullOrWhiteSpace(role))
        {
            isLoaded = true;
            return;
        }

        if (role.Equals("member", StringComparison.OrdinalIgnoreCase))
        {
            var member = await _memberService.GetMemberAsync(userId);
            if (member is not null)
            {
                CurrentUser.SetUserAsMember(member);
                CurrentUserRole = "member";
                CurrentUserMembership = member.MembershipType;
                _cache.Set("currentUser", member, TimeSpan.FromMinutes(60));
                isLoaded = true;
            }
        }

        else if (role.Equals("employee", StringComparison.OrdinalIgnoreCase))
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(userId);
            
            if (employee is not null)
            {
                CurrentUser.SetUserAsEmployee(employee);
                CurrentUserRole = "employee";
                _cache.Set("currentUser", employee, TimeSpan.FromMinutes(60));
                isLoaded = true;
            }
        }
    }

    /// <summary>
    /// AO: Clears the token and user from cache. 
    /// </summary>
    public async Task Logout()
    {
        _tokenService.Clear();
        ClearCurrentUser();
    }

    /// <summary>
    /// Helper for logout
    /// </summary>
    public void ClearCurrentUser()
    {
        _cache.Remove("currentUser");
    }

    public void SetAuthorization(bool authorized)
    {
        isAuthorized = authorized;
    }

    public string DisplayCurrentUserName()
    {
        if (CurrentUser?.Member is not null)
        {
            return $"{CurrentUser.Member.GivenName} " + $"{CurrentUser.Member.FamilyName}";
        }

        if (CurrentUser?.Employee is not null)
        {
            return $"{CurrentUser.Employee.GivenName} " + $"{CurrentUser.Employee.FamilyName}";
        }
        return "Fejl: Intet navn fundet";
    }


    public bool IsPremium => CurrentUserRole?.Equals("premium", StringComparison.OrdinalIgnoreCase) == true;
}