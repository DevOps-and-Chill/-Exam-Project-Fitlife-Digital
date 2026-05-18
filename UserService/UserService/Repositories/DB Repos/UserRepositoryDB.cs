using Microsoft.EntityFrameworkCore;
using UserServiceAPI.Data;
using UserServiceAPI.Models;
using UserServiceAPI.Repositories.Interfaces;

namespace UserServiceAPI.Repositories
{
    public class UserRepositoryDB : IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepositoryDB(UserDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> LoadTestData()
        {
            _context.Users.AddRange(TestData.EmployeeTestData.employees);
            _context.Users.AddRange(TestData.MemberTestData.members);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetUserById(string userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == userId);
            return user;
        }

        public async Task<string?> GetUserIdByEmail(string email)
        {
            return await _context.Users
                    .Where(u => u.Email == email)
                    .Select(u => u.Id)
                    .SingleOrDefaultAsync();
        }
    }
}
