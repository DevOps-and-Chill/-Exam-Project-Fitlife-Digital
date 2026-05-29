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

        /// <summary>
        /// Retrieves all User entities from the database context asynchronously.
        /// </summary>
        /// <remarks>Executes a query via Entity Framework Core and materializes all User entities into
        /// memory. The operation may throw exceptions from the underlying data provider.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of User objects.</returns>
        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Asynchronously adds predefined employee and member users to the data context and saves the changes.
        /// </summary>
        /// <remarks>Exceptions thrown by SaveChangesAsync propagate to the caller.</remarks>
        /// <returns>True if the users were persisted successfully.</returns>
        public async Task<bool> LoadTestData()
        {
            _context.Users.AddRange(TestData.EmployeeTestData.employees);
            _context.Users.AddRange(TestData.MemberTestData.members);
            await _context.SaveChangesAsync();

            return true;
        }
        /// <summary>
        /// Asynchronously retrieves the user with the specified identifier.
        /// </summary>
        /// <param name="userId">The identifier of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the User if found; otherwise, null.</returns>
        public async Task<User?> GetUserById(string userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == userId);
            return user;
        }
        /// <summary>
        /// Gets the identifier of the user with the specified email.
        /// </summary>
        /// <param name="email">Email address of the user to locate.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user's identifier if found;
        /// otherwise null.</returns>
        public async Task<string?> GetUserIdByEmail(string email)
        {
            return await _context.Users
                    .Where(u => u.Email == email)
                    .Select(u => u.Id)
                    .SingleOrDefaultAsync();
        }
    }
}