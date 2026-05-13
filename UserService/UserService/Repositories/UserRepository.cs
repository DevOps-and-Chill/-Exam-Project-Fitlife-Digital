using UserServiceAPI.Models;
using UserServiceAPI.Repositories.Interfaces;

namespace UserServiceAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        List<User> = new List<User>();
        public Task<bool> CancelMembershipForMember(Guid userId)
        {
            
        }

        public Task<bool> DeleteUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EndEmploymentForEmployee(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllEmployees()
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllMembers()
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetUsersInExerciseGymByRole(Guid exerciseGymId, string role)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetEmployeeAsManagerForExerciseGym(Guid exerciseGymId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetUserAsInactive(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpsertUser()
        {
            throw new NotImplementedException();
        }
    }
}
