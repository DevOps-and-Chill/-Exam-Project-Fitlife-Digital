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
            CurrentUserRole = "member";
            CurrentUserMembership = member.MembershipType;

            if (member is not null)
            {
                CurrentUser.SetUserAsMember(member);

                _cache.Set("currentUser", member, TimeSpan.FromMinutes(60));
            }
        }

        else if (role.Equals("employee", StringComparison.OrdinalIgnoreCase))
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(userId);
            CurrentUserRole = "employee";
            if (employee is not null)
            {
                CurrentUser.SetUserAsEmployee(employee);

                _cache.Set("currentUser", employee, TimeSpan.FromMinutes(60));
            }
        }
    }

    /// <summary>
    /// AO: Clears the token and user from cache. 
    /// </summary>
    public void Logout()
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
        string displayName = "";
        if (CurrentUser != null)
        {
            if (CurrentUser.Member != null)
            {
                return displayName = $"{CurrentUser.Member.GivenName}" + $"{CurrentUser.Member.FamilyName}";
            }
            return displayName = $"{CurrentUser.Employee.GivenName}" + $"{CurrentUser.Employee.FamilyName}";
        }
        return displayName = "Fejl: Intet navn fundet";
    }

    public bool IsPremium => CurrentUserRole?.Equals("premium", StringComparison.OrdinalIgnoreCase) == true;
}