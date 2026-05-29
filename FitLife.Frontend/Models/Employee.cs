namespace FitLife.Frontend.Models;

public class Employee
{
    public string Id { get; set; } = "";

    public string GivenName { get; set; } = "";

    public string FamilyName { get; set; } = "";
    
    public string? fullName { get; set; } = "";
    public string? Description { get; set; } = "";
    
    public bool IsAvailable {get; set;}

    public string Email { get; set; } = "";

    public string Affiliation { get; set; } = Guid.NewGuid().ToString();

    public bool ActiveUser { get; set; }

    public bool IsPT { get; set; }

    public string EmployeeRoleName { get; set; } = "";
}