using FitLife.Frontend.Models;
using FitLife.Frontend.Models.DTOs;
using System.Diagnostics.Metrics;
using System.Net.Http.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FitLife.Frontend.Services;

public class MemberService
{
    private readonly HttpClient _httpClientUserService;
    private readonly HttpClient _httpClientAuthService;
    private readonly TokenService _tokenService;

    public MemberService(IHttpClientFactory httpClientFactory, TokenService tokenService)
    {
        _httpClientUserService = httpClientFactory.CreateClient("UserService");
        _httpClientAuthService = httpClientFactory.CreateClient("AuthService");
        _tokenService = tokenService;
    }

    public Member DraftMember { get; set; } = new();
    public CredentialDTO DraftCredential { get; set; } = new();

    public void ResetDraftMember()
    {
        DraftMember = new Member();
    }

    // Henter medlemmer direkte fra UserService.
    // Kalder endpoint:
    // GET /member/GetAllMembers
    public async Task<List<Member>> GetMembersAsync()
    {
        try
        {
            _tokenService.AttachToken(_httpClientUserService);
            var members = await _httpClientUserService.GetFromJsonAsync<List<Member>>("member/GetAllMembers");

            return members ?? new List<Member>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke hente medlemmer fra UserService. Fejl: {ex.Message}", ex);
        }
    }
    
    public async Task<Member?> GetMemberByIdAsync(string id)
    {
        try
        {
            _tokenService.AttachToken(_httpClientUserService);
            return await _httpClientUserService.GetFromJsonAsync<Member>($"member/GetMemberById/{id}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke hente medlem med id {id}. Fejl: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Get member from userservice
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>Memberobject or null</returns>
    public async Task<Member?> GetMemberAsync(string userId)
    {
        try
        {
            _tokenService.AttachToken(_httpClientUserService);
            return await _httpClientUserService.GetFromJsonAsync<Member>($"member/GetMemberById/{userId}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> CreateNewMember(Member newMember, CredentialDTO credentials)
    {
        try
        {
            Member memberCreated = await CreateMemberAsync(newMember);
            string memberId = memberCreated.Id;
            credentials.UserId = memberId;
            var response = await _httpClientAuthService.PostAsJsonAsync("auth/RegisterCredentials", credentials);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }
        catch
        {
            throw;

        }
        return false;
    }

    // Opretter eller opdaterer et medlem via UserService.
    // Kalder endpoint:
    // POST /member/UpsertMember
    public async Task<Member> CreateMemberAsync(Member member)
    {
        try
        {
            
            var response = await _httpClientUserService.PostAsJsonAsync("member/UpsertMember", member);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new Exception($"UserService returnerede fejl: {(int)response.StatusCode} {response.ReasonPhrase}. {errorMessage}");
            }

            var createdMember = await response.Content.ReadFromJsonAsync<Member>();

            return createdMember ?? member;
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke oprette medlem via UserService. Fejl: {ex.Message}", ex);
        }
    }

    public async Task<Member> SaveMember(Member member)
    {
        _tokenService.AttachToken(_httpClientUserService);
        var response = await _httpClientUserService.PostAsJsonAsync("member/UpsertMember", member);
        response.EnsureSuccessStatusCode();
        return member;
    }

    // Midlertidig prototypefunktion.
    // TODO: Skal senere integreres med rigtig betalingsservice/payment provider.
    public string UpdatePaymentMethod(Member member)
    {
        return $"Betalingsmiddel for {member.GivenName} {member.FamilyName} kan opdateres senere via betalingsintegration.";
    }
}