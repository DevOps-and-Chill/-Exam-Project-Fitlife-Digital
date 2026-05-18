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
            var employee = await _context.Users
            .OfType<Employee>()
            .FirstOrDefaultAsync(m => m.Id == userId);

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
            var employee = await _context.Users
                .OfType<Employee>()
                .FirstOrDefaultAsync(m => m.Id == userId);

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
            var employee = await _context.Users
                .OfType<Employee>()
                .FirstOrDefaultAsync(m => m.Id == userId);

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
            var employee = await _context.Users
                .OfType<Employee>()
                .FirstOrDefaultAsync(m => m.Id == userId);

            if (employee == null)
            {
                return null;
            }

            employee.SetAsManager();

            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task<Employee> UpsertEmployee(Employee employee)
        {
            _context.Users.Add(employee);

            await _context.SaveChangesAsync();

            return employee;
        }
    }
}
