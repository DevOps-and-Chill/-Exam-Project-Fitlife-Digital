using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using UserServiceAPI.Data;
using UserServiceAPI.Models;
using UserServiceAPI.Repositories.Interfaces;

namespace UserServiceAPI.Repositories
{
    public class EmployeeRepositoryDB : IEmployeeRepository
    {
        private readonly UserDbContext _context;

        public EmployeeRepositoryDB(UserDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Finds employee based on id and removes from DB.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Returns object that has been removed</returns>
        public async Task<Employee> DeleteEmployee(string userId)
        {
            var employee = await GetEmployeeById(userId);

            if (employee == null)
            {
                return null;
            }

            _context.Users.Remove(employee);

            await _context.SaveChangesAsync();

            return employee;
        }

        /// <summary>
        /// Sets the property ActiveEmployment as false  
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Returns updated employee-object</returns>
        public async Task<Employee> EndEmploymentForEmployee(string userId)
        {
            var employee = await GetEmployeeById(userId);

            if (employee == null)
            {
                return null;
            }

            employee.EndEmployment();

            await _context.SaveChangesAsync();

            return employee;
        }

        /// <summary>
        /// Used to get a list of all employees in the db
        /// </summary>
        /// <returns>List of employees</returns>
        public async Task<List<Employee>> GetAllEmployees()
        {
            return await _context.Users.OfType<Employee>().ToListAsync();
        }

        /// <summary>
        /// Used to set the property ActiveUser to false
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Updated employee-object</returns>
        public async Task<Employee> SetAccountAsInactive(string userId)
        {
            var employee = await GetEmployeeById(userId);

            if (employee == null)
            {
                return null;
            }

            employee.SetUserAsInactive();

            await _context.SaveChangesAsync();

            return employee;
        }

        /// <summary>
        /// Sets the property employeeRole to Manager
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Returns the updated employee</returns>
        public async Task<Employee> SetEmployeeAsManager(string userId)
        {
            var employee = await GetEmployeeById(userId);

            if (employee == null)
            {
                return null;
            }

            employee.SetAsManager();

            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task<Employee?> GetEmployeeById(string userId)
        {
            var employee = await _context.Users
                .OfType<Employee>()
                .FirstOrDefaultAsync(m => m.Id == userId);
            return employee;
        }

        public async Task<Employee> UpsertEmployee(Employee employee)
        {
            bool emailExists = await _context.Users
               .AnyAsync(u =>
                   u.Email == employee.Email &&
                   u.Id != employee.Id);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    $"Email '{employee.Email}' is already in use");
            }
            Employee ?existingEmployee = await GetEmployeeById(employee.Id);

            if (existingEmployee is null)
            {
                _context.Users.Add(employee);
            }
            else
            {
                existingEmployee.UpdateUserInformation(
                    employee.RoleName,
                    employee.GivenName,
                    employee.FamilyName,
                    employee.Address,
                    employee.Telephone,
                    employee.Email,
                    employee.Affiliation,
                    employee.ActiveUser);

                existingEmployee.UpdateEmplyoment(
                    employee.EmployeeRoleName,
                    employee.IsPT,
                    employee.EndDate,
                    employee.ActiveEmployment);
            }

            await _context.SaveChangesAsync();

            return existingEmployee ?? employee;
        }

        public async Task<List<Employee>> GetEmployeesByAffiliation(Guid affiliationId)
        {
            return await _context.Users
                .OfType<Employee>()
                .Where(e => e.Affiliation == affiliationId)
                .ToListAsync();
        }
    }
}
