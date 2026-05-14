using UserServiceAPI.Models;
using UserServiceAPI.Models.Enums;
using UserServiceAPI.Repositories.Interfaces;
using UserServiceAPI.TestData;

namespace UserServiceAPI.Repositories
{
    public class UserRepositoryMOCK : IUserRepository
    {
        List<User> users = UserTestData.users;
        List<Member> members = MemberTestData.members;
        List<Employee> employees = EmployeeTestData.employees;

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
                
            }


            return Task.FromResult(existingUser);
        }
        /// <summary>
        /// Used to get all users connected to a facility
        /// </summary>
        /// <param name="affiliationId"></param>
        /// <returns>List of Users</returns>
        public Task<List<User>> GetUsersByAffiliation(Guid affiliationId)
        {
            var listByAffiliation = users.Where(u => u.Affiliation == affiliationId).ToList();

            return Task.FromResult(listByAffiliation);
        }
    }
}
