using UserServiceAPI.Models;

namespace UserServiceAPI.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        public Task<List<Employee>> GetAllEmployees();
        public Task<Employee> EndEmploymentForEmployee(string userId);
        public Task<Employee> SetEmployeeAsManager(string userId);
        public Task<Employee> UpsertEmployee(Employee employee);
        public Task<Employee> DeleteEmployee(string userId);
        public Task<Employee> SetAccountAsInactive(string userId);
        public Task<Employee?> GetEmployeeById(string id);
        public Task<List<Employee>> GetEmployeesByAffiliation(Guid affiliationId);
    }
}
