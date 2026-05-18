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

        public Task<List<User>> GetUsersByAffiliation(Guid affiliationId)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetUsersInExerciseGymByRole(Guid exerciseGymId, string role)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> LoadTestData()
        {
            _context.Users.AddRange(TestData.EmployeeTestData.employees);
            _context.Users.AddRange(TestData.MemberTestData.members);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
