using UserServiceAPI.Models;

namespace UserServiceAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllUsers();
        public Task<bool> LoadTestData();
        public Task<User?> GetUserById(string userId);
        public Task<string?> GetUserIdByEmail(string email);
    }
}
