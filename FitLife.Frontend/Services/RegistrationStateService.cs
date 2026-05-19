using FitLife.Frontend.Models;

namespace FitLife.Frontend.Services;

public class RegistrationStateService
{
    public Member Member { get; set; } = new()
    {
        BirthDate = DateTime.Today,
        StartDate = DateTime.Today,
        EndDate = DateTime.Today.AddYears(1),
        RoleName = "Member",
        PartitionKey = "users",
        ActiveUser = true,
        ActiveMembership = true
    };

    public string SelectedCenterName { get; set; } = "";

    public MembershipType? SelectedMembership { get; set; }

    public bool PersonalTrainingAddon { get; set; }

    public bool DigitalTrainingAddon { get; set; }

    public PaymentInfo Payment { get; set; } = new();

    public decimal TotalPrice =>
        (SelectedMembership?.MonthlyPrice ?? 0) +
        (PersonalTrainingAddon ? 150 : 0) +
        (DigitalTrainingAddon ? 50 : 0);
}