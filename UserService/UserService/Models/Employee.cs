using UserServiceAPI.Models.Enums;

namespace UserServiceAPI.Models
{
    public class Employee : User
    {
        public EmployeeRole EmployeeRoleName { get; set; }
        public bool isPT { get; set; }
        public DateTime StartDate { get; init; }

        public DateTime EndDate { get; set; }

        public bool ActiveEmployment { get; set; } = true;

        public Employee(
            UserRole userRole,
            string givenName,
            string familyName,
            string address,
            string telephone,
            string email,
            Guid affiliation,
            bool activeUser,
            EmployeeRole roleName,
            bool isPT,
            DateTime endDate)
            : base(
                  userRole,
                  givenName,
                  familyName,
                  address,
                  telephone,
                  email,
                  affiliation,
                  activeUser)
        {
            EmployeeRoleName = roleName;
            this.isPT = isPT;
            EndDate = endDate;
        }

        public void EndEmployment()
        {
            ActiveEmployment = false; 
        }
    }
}
