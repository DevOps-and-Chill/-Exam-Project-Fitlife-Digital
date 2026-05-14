using UserServiceAPI.Models;
using UserServiceAPI.Models.Enums;
using UserServiceAPI.Repositories.Interfaces;
using UserServiceAPI.TestData;

namespace UserServiceAPI.Repositories
{
    public class EmployeeRepositoryMOCK : IEmployeeRepository
    {
        List<Employee> employees = EmployeeTestData.employees;
        public Task<bool> EndEmploymentForEmployee(Guid userId)
        {
            var employeeToUpdate = employees.SingleOrDefault(m => m.Id == userId);

            if (employeeToUpdate == null)
            {
                return Task.FromResult(false);
            }

            employeeToUpdate.EndEmployment();

            return Task.FromResult(true);
        }

        public Task<List<User>> GetAllEmployees()
        {
            List<User> result = employees
                .Cast<User>()
                .ToList();

            return Task.FromResult(result);
        }

        public Task<bool> SetEmployeeAsManager(Guid userId)
        {
            var employee = employees.SingleOrDefault(e => e.Id == userId);

            if (employee == null)
            {
                return Task.FromResult(false);
            }

            employee.SetAsManager();

            return Task.FromResult(true);
        }
    }
}
