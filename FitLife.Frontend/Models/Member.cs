namespace FitLife.Frontend.Models;

public class Member
{
    public int Id { get; set; }

    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";

    public string Address { get; set; } = "";
    public string PostalCode { get; set; } = "";
    public string City { get; set; } = "";

    public string Email { get; set; } = "";
    public string PhoneNumber { get; set; } = "";

    public DateTime BirthDate { get; set; }

    public string MembershipType { get; set; } = "";
    public string PaymentStatus { get; set; } = "";

    public DateTime MemberSince { get; set; }

    public string CurrentPersonalTrainer { get; set; } = "";

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