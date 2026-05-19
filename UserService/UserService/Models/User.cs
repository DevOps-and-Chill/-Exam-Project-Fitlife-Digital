using UserServiceAPI.Models.Enums;

namespace UserServiceAPI.Models
{
    public abstract class User
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        //AO: Used in cosmos. Ensures correct partitioning. Enables later change in partitioning strategy, ie. partitioning by affiliation 
        public string PartitionKey { get; set; } = "users";

        public UserRole RoleName { get; set; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Address { get; set; }

        public string Telephone { get; set; }

        public string Email { get; set; }

        //AO: Might later be changed to string if necessary for setup of db in facilityservice. 
        public Guid Affiliation {  get; set; }

        public bool ActiveUser { get; set; }

        public DateTime DateCreated { get; init; } = DateTime.Now;

        public DateTime DateModified { get; set; } = DateTime.Now;

        protected User()
        {

        }
        public User(
             UserRole roleName,
             string givenName,
             string familyName,
             DateTime birthDate,
             string address,
             string telephone,
             string email,
             Guid affiliation,
             bool activeUser)
        {
            RoleName = roleName;
            GivenName = givenName;
            FamilyName = familyName;
            BirthDate = birthDate;
            Address = address;
            Telephone = telephone;
            Email = email;
            Affiliation = affiliation;
            ActiveUser = activeUser;
        }

        public void SetUserAsInactive()
        {
            if (ActiveUser)
                ActiveUser = false;
            else
                return;
        }

        public void UpdateUserInformation(
            UserRole userRole,
            string givenName,
            string familyName,
            DateTime birthDate,
            string address,
            string telephone,
            string email,
            Guid affiliation,
            bool activeUser)
        {
            RoleName = userRole;
            GivenName = givenName;
            FamilyName = familyName;
            BirthDate = birthDate;
            Address = address;
            this.Telephone = telephone;
            Email = email;
            Affiliation = affiliation;
            ActiveUser = activeUser;
            DateModified = DateTime.Now;
        }

    }
}
