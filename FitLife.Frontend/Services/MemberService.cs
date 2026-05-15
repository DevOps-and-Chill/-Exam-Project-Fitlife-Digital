using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class MemberService
{
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
            MembershipType = "Plus",
            PaymentStatus = "Betalt",
            MemberSince = DateTime.Today.AddMonths(-6),
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

    public List<Member> GetMembers()
    {
        return _members;
    }

    public string CreateMember(Member member)
    {
        member.MemberSince = DateTime.Today;
        _members.Add(member);

        return $"Medlemmet {member.FirstName} {member.LastName} er oprettet.";
    }

    public string SaveMember(Member member)
    {
        return $"Ændringer for {member.FirstName} {member.LastName} er gemt.";
    }

    public string UpdatePaymentMethod(Member member)
    {
        return $"Betalingsmiddel for {member.FirstName} {member.LastName} kan opdateres senere via betalingsintegration.";
    }
}