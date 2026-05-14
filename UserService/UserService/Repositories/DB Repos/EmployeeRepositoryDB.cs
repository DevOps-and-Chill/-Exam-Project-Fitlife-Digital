using UserServiceAPI.Models;
using UserServiceAPI.Repositories.Interfaces;

namespace UserServiceAPI.Repositories
{
    public class EmployeeRepositoryDB : IEmployeeRepository
    {
        public Task<bool> EndEmploymentForEmployee(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllEmployees()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetEmployeeAsManager(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
