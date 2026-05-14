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

        public Task<bool> DeleteUser(Guid userId)
        {
            throw new NotImplementedException();
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

        public Task<bool> SetUserAsInactive(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<User> UpsertUser(User user)
        {
            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return user;
        }
    }
}
