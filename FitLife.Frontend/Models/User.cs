namespace FitLife.Frontend.Models
{
    public class User
    {
        public Member Member { get; set; } = null;
        public Employee Employee { get; set; } = null;

        public void SetUserAsMember(Member member)
        {
            Member = member;
            Employee = null;
        }
        public void SetUserAsEmployee(Employee employee)
        {
            Employee = employee;
            Member = null;
        }

        public string GetUserType()
        {
            if (Member is not null)
            {
                return "member";
            }

            if (Employee is not null)
            {
                return "employee";
            }

            return "";
        }
    }
}
