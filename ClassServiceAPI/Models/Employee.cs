namespace ClassServiceAPI.Models;

public class Employee
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string GivenName { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
}