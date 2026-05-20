namespace FitLife.Frontend.Models;

public class Employee
{
    public string Id { get; set; } = "";

    public string GivenName { get; set; } = "";

    public string FamilyName { get; set; } = "";

    public string Email { get; set; } = "";

    public Guid Affiliation { get; set; }

    public bool ActiveUser { get; set; }

    public bool IsPT { get; set; }

    public string EmployeeRoleName { get; set; } = "";
}