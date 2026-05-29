namespace ClassServiceAPI.Models;

public class Member
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string GivenName { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
}