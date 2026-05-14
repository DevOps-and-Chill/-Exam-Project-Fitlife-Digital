using UserServiceAPI.Models;
using UserServiceAPI.Repositories.Interfaces;

namespace UserServiceAPI.Repositories
{
    public class UserRepositoryDB : IUserRepository
    {
        public Task<bool> DeleteUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetUsersByAffiliation(Guid affiliationId)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetUsersInExerciseGymByRole(Guid exerciseGymId, string role)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetUserAsInactive(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpsertUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
