namespace FitLife.Frontend.Models;

public class Member
{
    public string Id { get; set; } = "";

    public string PartitionKey { get; set; } = "users";

    public string RoleName { get; set; } = "Member";

    public string GivenName { get; set; } = "";

    public string FamilyName { get; set; } = "";

    public string Address { get; set; } = "";

    public string Telephone { get; set; } = "";

    public string Email { get; set; } = "";

    public Guid Affiliation { get; set; }

    public bool ActiveUser { get; set; } = true;

    public DateTime BirthDate { get; set; }

    public string MembershipType { get; set; } = "";

    public string MembershipOptional { get; set; } = "";

    public DateTime StartDate { get; set; } = DateTime.Today;

    public DateTime EndDate { get; set; }

    public bool ActiveMembership { get; set; } = true;

    public DateTime DateCreated { get; set; } = DateTime.Today;

    public DateTime DateModified { get; set; } = DateTime.Today;

    public int Age
    {
        get
        {
            var today = DateTime.Today;
            var age = today.Year - BirthDate.Year;

            if (BirthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}