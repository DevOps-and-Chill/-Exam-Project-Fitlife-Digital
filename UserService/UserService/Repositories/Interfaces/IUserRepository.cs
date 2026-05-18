using UserServiceAPI.Models;

namespace UserServiceAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllUsers();
        public Task<List<User>> GetUsersInExerciseGymByRole(Guid exerciseGymId, string role);
        public Task<List<User>> GetUsersByAffiliation(Guid affiliationId);
        public Task<bool> LoadTestData();
        public Task<User?> GetUserById(string userId);
        public Task<string?> GetUserIdByEmail(string email);
    }
}