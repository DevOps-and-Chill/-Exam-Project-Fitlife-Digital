namespace FitLife.Frontend.Models;

public class Member
{
    // CosmosDB/UserService bruger string Id.
    // Guid bruges som standard når nye medlemmer oprettes.
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string PartitionKey { get; set; } = "users";

    public string RoleName { get; set; } = "Member";

    public string GivenName { get; set; } = "";

    public string FamilyName { get; set; } = "";

    public string Address { get; set; } = "";

    public string Telephone { get; set; } = "";

    public string Email { get; set; } = "";

    // TODO:
    // Skal senere kobles til rigtige centre/facilities.
    public string Affiliation { get; set; } = Guid.NewGuid().ToString();

    public bool ActiveUser { get; set; } = true;

    public DateTime BirthDate { get; set; }

    // Backend bruger string værdier.
    // Fx "Standard" eller "Premium".
    public string MembershipType { get; set; }


    public string MembershipOptional { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool ActiveMembership { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    // Computed property.
    // Gemmes normalt ikke i databasen,
    // men bruges kun i frontend/UI.
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