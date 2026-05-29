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

            _context.Employees.Remove(employee);

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
            return await _context.Employees.ToListAsync();
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
        /// <summary>
        /// Based on userid finds the employee
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>return the employee-object. Returns null if not found</returns>
        public async Task<Employee?> GetEmployeeById(string userId)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == userId);
            return employee;
        }

        /// <summary>
        /// Inserts or updates the employee-object. Ensures that email is unique
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>Updated or new object</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<Employee> UpsertEmployee(Employee employee)
        {
            var existing = await _context.Employees
                .FirstOrDefaultAsync(u =>
                    u.Email == employee.Email &&
                    u.Id != employee.Id);

            bool emailExists = existing != null;

            if (emailExists)
            {
                throw new InvalidOperationException(
                    $"Email '{employee.Email}' is already in use");
            }
            Employee ?existingEmployee = await GetEmployeeById(employee.Id);

            if (existingEmployee is null)
            {
                _context.Employees.Add(employee);
            }
            else
            {
                existingEmployee.UpdateUserInformation(
                    employee.RoleName,
                    employee.GivenName,
                    employee.FamilyName,
                    employee.BirthDate,
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

        /// <summary>
        /// Finds the employees based on affiliation (typically an exerciseGym) 
        /// </summary>
        /// <param name="affiliationId"></param>
        /// <returns>Returns list of employees</returns>
        public async Task<List<Employee>> GetEmployeesByAffiliation(Guid affiliationId)
        {
            return await _context.Employees.Where(e => e.Affiliation == affiliationId).ToListAsync();
        }
    }
}
