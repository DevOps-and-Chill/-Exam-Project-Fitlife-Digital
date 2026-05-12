using UserServiceAPI.Models;
using UserServiceAPI.Models.Enums;
using UserServiceAPI.Repositories.Interfaces;

namespace UserServiceAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static readonly Guid fitnessCenterId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
        private static readonly Guid fitnessCenterId2 = Guid.Parse("11111111-1111-1111-1111-111111111112");

        List<Member> members = new List<Member>
        {
            new Member(
                UserRole.Member,
                "Mikkel",
                "Andersen",
                "Vestergade 12, 8000 Aarhus C",
                "22334455",
                "mikkel.andersen@mail.dk",
                fitnessCenterId1,
                true,
                MembershipType.Standard,
                MembershipOptional.Swimming
            ),

            new Member(
                UserRole.Member,
                "Sofie",
                "Nielsen",
                "Nørrebrogade 45, 8200 Aarhus N",
                "33445566",
                "sofie.nielsen@mail.dk",
                fitnessCenterId1,
                true,
                MembershipType.Premium,
                MembershipOptional.Pizza
            ),

            new Member(
                UserRole.Member,
                "Lucas",
                "Jensen",
                "Parkvej 8, 8260 Viby J",
                "44556677",
                "lucas.jensen@mail.dk",
                fitnessCenterId1,
                true,
                MembershipType.Standard,
                MembershipOptional.Pizza
            ),

            new Member(
                UserRole.Member,
                "Emma",
                "Christensen",
                "Skovvej 19, 8240 Risskov",
                "55667788",
                "emma.christensen@mail.dk",
                fitnessCenterId2,
                true,
                MembershipType.Premium,
                MembershipOptional.Swimming
            ),

            new Member(
                UserRole.Member,
                "Oliver",
                "Madsen",
                "Engtoften 3, 8381 Tilst",
                "66778899",
                "oliver.madsen@mail.dk",
                fitnessCenterId2,
                true,
                MembershipType.Standard,
                MembershipOptional.Swimming
            ),

            new Member(
                UserRole.Member,
                "Freja",
                "Larsen",
                "Bakkevej 27, 8270 Højbjerg",
                "77889900",
                "freja.larsen@mail.dk",
                fitnessCenterId2,
                true,
                MembershipType.Premium,
                MembershipOptional.Pizza
            )
        };

        List<Employee> employees = new List<Employee>
        {
            new Employee(
                UserRole.Employee,
                "Andreas",
                "Pedersen",
                "Søndergade 5, 8000 Aarhus C",
                "11223344",
                "andreas.pedersen@fitlife.dk",
                fitnessCenterId1,
                true,
                EmployeeRole.Manager,
                false
            ),

            new Employee(
                UserRole.Employee,
                "Julie",
                "Mortensen",
                "Kirkegade 14, 8230 Åbyhøj",
                "22334411",
                "julie.mortensen@fitlife.dk",
                fitnessCenterId1,
                true,
                EmployeeRole.Staffmember,
                true
            ),

            new Employee(
                UserRole.Employee,
                "Kasper",
                "Hansen",
                "Møllevej 9, 8250 Egå",
                "33441122",
                "kasper.hansen@fitlife.dk",
                fitnessCenterId2,
                true,
                EmployeeRole.Staffmember,
                false
            ),

            new Employee(
                UserRole.Employee,
                "Laura",
                "Thomsen",
                "Lindevej 21, 8520 Lystrup",
                "44552233",
                "laura.thomsen@fitlife.dk",
                fitnessCenterId2,
                true,
                EmployeeRole.Staffmember,
                true
            )
        };

        List<User> users = new List<User>();

        public UserRepository()
        {
            users.AddRange(members.Cast<User>());
            users.AddRange(employees.Cast<User>());
        }



        /// <summary>
        /// Returns true when membership is set to false. 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<bool> CancelMembershipForMember(Guid userId)
        {
            var memberToCancel = members.SingleOrDefault(m => m.Id == userId);

            if(memberToCancel == null)
            {
                return Task.FromResult(false);
            }

            memberToCancel.CancelMembership();

            return Task.FromResult(true); 
        }

        public Task<bool> DeleteUser(Guid userId)
        {
            string userRole = users.SingleOrDefault(u => u.Id == userId).RoleName.ToString();
            if(userRole == null)
            {
                return Task.FromResult(false);
            }
            if(userRole == "Member")
            {
                var memberToRemove = members.SingleOrDefault(m => m.Id == userId);
                members.Remove(memberToRemove);
                return Task.FromResult(true);
            }

            if (userRole == "Employee")
            {
                var employeeToRemove = employees.SingleOrDefault(m => m.Id == userId);
                employees.Remove(employeeToRemove);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<bool> EndEmploymentForEmployee(Guid userId)
        {
            var employeeToUpdate = employees.SingleOrDefault(m => m.Id == userId);

            if (employeeToUpdate == null)
            {
                return Task.FromResult(false);
            }

            employeeToUpdate.EndEmployment();

            return Task.FromResult(true);
        }

        public Task<List<User>> GetAllEmployees()
        {
            List<User> result = employees
                .Cast<User>()
                .ToList();

            return Task.FromResult(result);
        }

        public Task<List<User>> GetAllMembers()
        {
            List<User> result = members
                .Cast<User>()
                .ToList();

            return Task.FromResult(result);
        }

        public Task<List<User>> GetAllUsers()
        {
            return Task.FromResult(users);
        }

        public Task<List<User>> GetUsersInExerciseGymByRole(Guid exerciseGymId, string role)
        {
            List<User> usersInExerciseGym = members
                .Where(m => m.Affiliation == exerciseGymId)
                .Cast<User>()
                .Concat(
                    employees.Where(e => e.Affiliation == exerciseGymId)
                )
                .ToList();

            return Task.FromResult(usersInExerciseGym);
        }

        public Task<bool> SetEmployeeAsManager(Guid userId)
        {
            var employee = employees.SingleOrDefault(e => e.Id == userId);

            if (employee == null)
            {
                return Task.FromResult(false);
            }

            employee.SetAsManager();

            return Task.FromResult(true);
        }

        public Task<bool> SetUserAsInactive(Guid userId)
        {
            var member = members.SingleOrDefault(e => e.Id == userId);

            if (member == null)
            {
                return Task.FromResult(false);
            }

            member.SetUserAsInactive();

            return Task.FromResult(true);
        }

        //AO: Mangler
        public Task<User> UpsertUser(User userToUpsert)
        {
            var existingUser = users
                .SingleOrDefault(u => u.Id == userToUpsert.Id);

            if (existingUser == null)
            {
                users.Add(userToUpsert);

                return Task.FromResult(userToUpsert);
            }

            existingUser.UpdateUserInformation(
                userToUpsert.RoleName,
                userToUpsert.GivenName,
                userToUpsert.FamilyName,
                userToUpsert.Address,
                userToUpsert.Telephone,
                userToUpsert.Email,
                userToUpsert.Affiliation,
                userToUpsert.ActiveUser);

            if(userToUpsert.RoleName.ToString() == "Member")
            {
                existingUser
            }


            return Task.FromResult(existingUser);
        }
    }


        
    }
}
