using System.Net.Http.Json;
using FitLife.Frontend.Models;

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
    // Frontend og backend bruger nu samme modelstruktur.
    public async Task<List<Member>> GetMembersAsync()
    {
        // Kalder endpoint:
        // GET /member/GetAllMembers
        var members =
            await _httpClient.GetFromJsonAsync<List<Member>>
            ("/member/GetAllMembers");

        // Hvis API returnerer null,
        // returneres en tom liste i stedet.
        return members ?? new List<Member>();
    }

    // Opretter et nyt medlem via UserService.
    // Denne metode sender Member-objektet direkte til backend API.
    public async Task<Member> CreateMemberAsync(Member member)
    {
        // Sender POST request til UserService endpoint:
        // POST /member/UpsertMember
        var response =
            await _httpClient.PostAsJsonAsync(
                "/member/UpsertMember",
                member);

        // Hvis request fejler,
        // kastes exception så frontend kan vise fejlbesked.
        response.EnsureSuccessStatusCode();

        // Backend returnerer det oprettede/opdaterede medlem.
        var createdMember =
            await response.Content.ReadFromJsonAsync<Member>();

        // Hvis backend returnerer null,
        // returneres originalt member objekt.
        return createdMember ?? member;
    }

    // Midlertidig lokal funktion.
    // TODO:
    // Senere bør denne sende PUT/PATCH request til UserService.
    public string SaveMember(Member member)
    {
        return $"Ændringer for {member.GivenName} {member.FamilyName} er gemt.";
    }

    // Midlertidig prototypefunktion.
    // TODO:
    // Skal senere integreres med rigtig betalingsservice/payment provider.
    public string UpdatePaymentMethod(Member member)
    {
        return $"Betalingsmiddel for {member.GivenName} {member.FamilyName} kan opdateres senere via betalingsintegration.";
    }
}