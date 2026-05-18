using UserServiceAPI.Models.Enums;

namespace UserServiceAPI.Models
{
    public class Employee : User
    {
        public EmployeeRole EmployeeRoleName { get; set; }
        public bool IsPT { get; set; }

        //AO: To be set when employee is created
        public DateTime StartDate { get; init; }

        public DateTime EndDate { get; set; }

        public bool ActiveEmployment { get; set; } = true;

        public Employee()
        {

        }
        public Employee(
            UserRole userRole,
            string givenName,
            string familyName,
            DateTime birthDate,
            string address,
            string telephone,
            string email,
            Guid affiliation,
            bool activeUser,
            EmployeeRole roleName,
            bool isPT)
            : base(
                  userRole,
                  givenName,
                  familyName,
                  birthDate,
                  address,
                  telephone,
                  email,
                  affiliation,
                  activeUser)
        {
            EmployeeRoleName = roleName;
            this.IsPT = isPT;
        }

        public void EndEmployment()
        {
            ActiveEmployment = false; 
        }

        public void SetAsManager()
        {
            if (!ActiveEmployment)
            {
                throw new InvalidOperationException(
                    "Inactive employees cannot become managers");
            }
            EmployeeRoleName = EmployeeRole.Manager;
        }

        public void UpdateEmplyoment(
            EmployeeRole newEmployeeRole,
            bool isPT, 
            DateTime newEnd,
            bool setActiveState)
        {
            EmployeeRoleName = newEmployeeRole;
            IsPT = isPT;
            EndDate = newEnd;
            ActiveEmployment = setActiveState;
        }
    }
}
