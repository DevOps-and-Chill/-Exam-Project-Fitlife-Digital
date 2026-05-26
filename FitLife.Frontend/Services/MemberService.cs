using FitLife.Frontend.Models;
using System.Diagnostics.Metrics;
using System.Net.Http.Json;

namespace FitLife.Frontend.Services;

public class MemberService
{
    // HttpClient bruges til at kommunikere med backend APIs.
    // Her bruges den til at kalde UserService.
    private readonly HttpClient _httpClient;

    public MemberService(IHttpClientFactory httpClientFactory)
    {
        // Opretter HttpClient med base URL fra Program.cs/appsettings.json.
        // Base URL peger på UserService.
        _httpClient = httpClientFactory.CreateClient("UserService");
    }

    // Henter medlemmer direkte fra UserService.
    // Kalder endpoint:
    // GET /member/GetAllMembers
    public async Task<List<Member>> GetMembersAsync()
    {
        try
        {
            var members =
                await _httpClient.GetFromJsonAsync<List<Member>>(
                    "/member/GetAllMembers");

            return members ?? new List<Member>();
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"Kunne ikke hente medlemmer fra UserService. Fejl: {ex.Message}",
                ex);
        }
    }
    
    public async Task<Member?> GetMemberByIdAsync(string id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Member>($"/member/GetMemberById/{id}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke hente medlem med id {id}. Fejl: {ex.Message}", ex);
        }
    }

  /// <summary>
  /// Get a member by string (guid as string) 
  /// </summary>
  /// <param name="userId"></param>
  /// <returns>Returns a member object from userservice</returns>
  /// <exception cref="Exception"></exception>
    public async Task<Member> GetMemberAsync(string userId)
    {
        try
        {
            var member = await _httpClient.GetFromJsonAsync<Member>($"/member/GetMemberById/{userId}");

            return member;
        }
        catch (Exception ex)
        {
            throw new Exception($"Kunne ikke hente medlem fra UserService. \n Errormessage: {ex.Message}", ex);
        }
    }

    // Opretter eller opdaterer et medlem via UserService.
    // Kalder endpoint:
    // POST /member/UpsertMember
    public async Task<Member> CreateMemberAsync(Member member)
    {
        try
        {
            var response =
                await _httpClient.PostAsJsonAsync(
                    "/member/UpsertMember",
                    member);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage =
                    await response.Content.ReadAsStringAsync();

                throw new Exception(
                    $"UserService returnerede fejl: {(int)response.StatusCode} {response.ReasonPhrase}. {errorMessage}");
            }

            var createdMember =
                await response.Content.ReadFromJsonAsync<Member>();

            return createdMember ?? member;
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"Kunne ikke oprette medlem via UserService. Fejl: {ex.Message}",
                ex);
        }
    }

    // Midlertidig lokal funktion.
    // TODO: Senere bør denne sende PUT/PATCH request til UserService.
    public string SaveMember(Member member)
    {
        return $"Ændringer for {member.GivenName} {member.FamilyName} er gemt.";
    }

    // Midlertidig prototypefunktion.
    // TODO: Skal senere integreres med rigtig betalingsservice/payment provider.
    public string UpdatePaymentMethod(Member member)
    {
        return $"Betalingsmiddel for {member.GivenName} {member.FamilyName} kan opdateres senere via betalingsintegration.";
    }
}