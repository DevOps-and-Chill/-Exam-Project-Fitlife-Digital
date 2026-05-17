using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class MemberService
{
    // Denne service håndterer medlemsdata i frontend-prototypen.
    // Service-laget fungerer som bindeled mellem UI og data.
    // TODO:
    // Senere skal denne service kommunikere med backend/API/database.

    // Midlertidig mock data til medlemmer
    // TODO:
    // Skal senere hentes fra backend/database.
    private readonly List<Member> _members = new()
    {
        new Member
        {
            FirstName = "Jonas",
            LastName = "Madsen",
            Address = "Munkeallé 27",
            PostalCode = "8000",
            City = "Aarhus",
            Email = "jonas@example.com",
            PhoneNumber = "12345678",
            BirthDate = new DateTime(1999, 4, 12),

            // Medlemsoplysninger
            MembershipType = "Plus",
            PaymentStatus = "Betalt",

            // Viser hvornår medlemmet blev oprettet
            MemberSince = DateTime.Today.AddMonths(-6),

            // Kobling til personlig træner
            CurrentPersonalTrainer = "Frederikke"
        },

        new Member
        {
            FirstName = "Frederik",
            LastName = "Nielsen",
            Address = "Vestergade 10",
            PostalCode = "8600",
            City = "Silkeborg",
            Email = "frederik@example.com",
            PhoneNumber = "22334455",
            BirthDate = new DateTime(1988, 9, 2),

            MembershipType = "Basis",
            PaymentStatus = "Mangler betaling",

            MemberSince = DateTime.Today.AddYears(-1),

            CurrentPersonalTrainer = ""
        },

        new Member
        {
            FirstName = "Charlotte",
            LastName = "Hansen",
            Address = "Stationsvej 4",
            PostalCode = "8700",
            City = "Horsens",
            Email = "charlotte@example.com",
            PhoneNumber = "87654321",
            BirthDate = new DateTime(1974, 1, 20),

            MembershipType = "Premium",
            PaymentStatus = "Betalt",

            MemberSince = DateTime.Today.AddYears(-3),

            CurrentPersonalTrainer = "Mikkel"
        }
    };

    // Returnerer alle medlemmer
    // Bruges af frontend til medlemsoversigter.
    // TODO:
    // Senere skal medlemmer hentes via API/database.
    public List<Member> GetMembers()
    {
        return _members;
    }

    // Opretter et nyt medlem
    // TODO:
    // Senere skal medlemmet gemmes i backend/database.
    public string CreateMember(Member member)
    {
        // Sætter oprettelsesdato automatisk
        member.MemberSince = DateTime.Today;

        // Tilføjer medlem til mock-listen
        _members.Add(member);

        return $"Medlemmet {member.FirstName} {member.LastName} er oprettet.";
    }

    // Gemmer ændringer på et medlem
    // TODO:
    // Senere skal ændringer sendes til backend/API.
    public string SaveMember(Member member)
    {
        return $"Ændringer for {member.FirstName} {member.LastName} er gemt.";
    }

    // Midlertidig betalingsmetode-funktion
    // TODO:
    // Skal senere integreres med rigtig betalingsservice/payment provider.
    public string UpdatePaymentMethod(Member member)
    {
        return $"Betalingsmiddel for {member.FirstName} {member.LastName} kan opdateres senere via betalingsintegration.";
    }
}