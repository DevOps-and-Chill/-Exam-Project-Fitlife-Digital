using UserServiceAPI.Models;
using UserServiceAPI.Models.Enums;
using UserServiceAPI.Repositories.Interfaces;
using UserServiceAPI.TestData;

namespace UserServiceAPI.Repositories
{
    public class EmployeeRepositoryMOCK : IEmployeeRepository
    {
        List<Employee> employees = EmployeeTestData.employees;

        public Task<Employee> DeleteEmployee(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EndEmploymentForEmployee(string userId)
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

        public Task<Employee> SetAccountAsInactive(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetEmployeeAsManager(string userId)
        {
            var employee = employees.SingleOrDefault(e => e.Id == userId);

            if (employee == null)
            {
                return Task.FromResult(false);
            }

            employee.SetAsManager();

            return Task.FromResult(true);
        }

        public Task<Employee> UpsertEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }

        Task<Employee> IEmployeeRepository.EndEmploymentForEmployee(string userId)
        {
            throw new NotImplementedException();
        }

        Task<List<Employee>> IEmployeeRepository.GetAllEmployees()
        {
            throw new NotImplementedException();
        }

        Task<Employee> IEmployeeRepository.SetEmployeeAsManager(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
