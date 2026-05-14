using UserServiceAPI.Models;

namespace UserServiceAPI.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        public Task<List<User>> GetAllEmployees();
        public Task<bool> EndEmploymentForEmployee(Guid userId);
        public Task<bool> SetEmployeeAsManager(Guid userId);
    }
}
