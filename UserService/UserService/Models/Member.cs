using UserServiceAPI.Models.Enums;

namespace UserServiceAPI.Models
{
    public class Member : User
    {
        //AO: Abonnementstype
        public MembershipType MembershipType { get; set; }

        //AO: Tilvalg til abonnement
        public MembershipOptional MembershipOptional { get; set; }

        //AO: Starttidspunkt for abonnement
        public DateTime StartDate { get; set; } = DateTime.Now;

        //AO: Sluttidspunkt for abonnement
        public DateTime EndDate { get; set; }

        public bool ActiveMembership { get; set; } = true;
        public Member()
        {

        }
        public Member(
            UserRole roleName,
            string givenName,
            string familyName,
            DateTime birthDate,
            string address,
            string telephone,
            string email,
            Guid affiliation,
            bool activeUser,
            MembershipType membershipType,
            MembershipOptional membershipOptional)
            : base(
                  roleName,
                  givenName,
                  familyName,
                  birthDate,
                  address,
                  telephone,
                  email,
                  affiliation,
                  activeUser)
        {
        MembershipType = membershipType;
        MembershipOptional = membershipOptional;
        }


        public void CancelMembership()
        {
            EndDate = DateTime.Now;
            ActiveMembership = false; 
        }

        public void UpdateMembership(
            MembershipType membershipType,
            MembershipOptional membershipOptional,
            DateTime newStartDate, 
            DateTime newEndDate)
        {
            MembershipType = membershipType;
            MembershipOptional = membershipOptional;
            StartDate = newStartDate;
            EndDate = newEndDate;
        }

    }
}
