using UserServiceAPI.Models.Enums;

namespace UserServiceAPI.Models
{
    public class User
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public UserRole RoleName { get; set; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public string Address { get; set; }

        public string Telephone { get; set; }

        public string Email { get; set; }

        public Guid Affiliation {  get; set; }

        public bool ActiveUser { get; set; }

        public DateTime DateCreated { get; init; } = DateTime.Now;

        public DateTime DateModified { get; set; } = DateTime.Now;


        public User(
             UserRole roleName,
             string givenName,
             string familyName,
             string address,
             string telephone,
             string email,
             Guid affiliation,
             bool activeUser)
        {
            RoleName = roleName;
            GivenName = givenName;
            FamilyName = familyName;
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
            string address,
            string telephone,
            string email,
            Guid affiliation,
            bool activeUser)
        {
            RoleName = userRole;
            GivenName = givenName;
            FamilyName = familyName;
            Address = address;
            this.Telephone = telephone;
            Email = email;
            Affiliation = affiliation;
            ActiveUser = activeUser;
            DateModified = DateTime.Now;
        }

    }
}
