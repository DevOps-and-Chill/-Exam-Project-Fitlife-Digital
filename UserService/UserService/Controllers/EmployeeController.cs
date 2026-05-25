using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserServiceAPI.Models;
using UserServiceAPI.Repositories;
using UserServiceAPI.Repositories.Interfaces;

namespace UserServiceAPI.Controllers
{
    [ApiController]
    [Route("employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeRepository employeeRepository, ILogger<EmployeeController> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all employees.
        /// </summary>
        /// <remarks>Returns 200 (OK) with the employee collection on success; 400 (Bad Request) with the
        /// exception on error.</remarks>
        /// <returns>An ActionResult that returns OK with the employee collection on success, or BadRequest with the exception on
        /// failure.</returns>
        [HttpGet("GetAllEmployees")]
        public async Task<ActionResult> GetAllEmployees()
        {
            _logger.LogInformation("Retrieving all employees");

            try
            {
                return Ok(await _employeeRepository.GetAllEmployees());
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to retrieve all employees: {Message}", ex.Message);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// End an employee's employment by user identifier.
        /// </summary>
        [HttpPut("EndEmploymentForEmployee/{userId}")]
        public async Task<ActionResult> EndEmployment(string userId)
        {
            _logger.LogInformation("Ending employment for employee {UserId}", userId);

            try
            {
                return Ok(await _employeeRepository.EndEmploymentForEmployee(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to end employment for employee {UserId}: {Message}", userId, ex.Message);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Sets the specified employee as a manager.
        /// </summary>
        [HttpPut("SetEmployeeAsManager/{userId}")]
        public async Task<ActionResult> SetEmployeeAsManager(string userId)
        {
            _logger.LogInformation("Setting employee {UserId} as manager", userId);

            try
            {
                return Ok(await _employeeRepository.SetEmployeeAsManager(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to set employee {UserId} as manager: {Message}", userId, ex.Message);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Upserts the specified employee and returns the created or updated employee.
        /// </summary>
        [HttpPost("UpsertEmployee")]
        public async Task<ActionResult> UpsertEmployee([FromBody] Employee employee)
        {
            _logger.LogInformation("Creating or updating employee {EmployeeId}", employee.Id);

            try
            {
                Employee updatedEmployee = await _employeeRepository.UpsertEmployee(employee);

                _logger.LogInformation("Successfully created or updated employee {EmployeeId}", employee.Id);

                return Ok(updatedEmployee);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(
                    "Invalid operation while creating or updating employee {EmployeeId}: {Message}",
                    employee.Id,
                    ex.Message);

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the employee identified by the provided userId.
        /// </summary>
        [HttpDelete("DeleteEmployee/{userId}")]
        public async Task<ActionResult> DeleteEmployee(string userId)
        {
            _logger.LogInformation("Deleting employee {UserId}", userId);

            try
            {
                Employee? deletedEmployee = await _employeeRepository.DeleteEmployee(userId);

                if (deletedEmployee == null)
                {
                    _logger.LogWarning("Employee with id {UserId} was not found", userId);
                    return NotFound("Employee not found");
                }

                _logger.LogInformation("Employee {UserId} deleted successfully", userId);

                return Ok(deletedEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete employee {UserId}", userId);

                return StatusCode(500, "An unexpected error occurred");
            }
        }

        /// <summary>
        /// Sets an employee account as inactive.
        /// </summary>
        [HttpPut("SetAccountAsInactive/{userId}")]
        public async Task<ActionResult> SetEmployeeAccountAsInactive(string userId)
        {
            _logger.LogInformation("Setting employee account as inactive for user {UserId}", userId);

            try
            {
                return Ok(await _employeeRepository.SetAccountAsInactive(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to deactivate account for user {UserId}: {Message}", userId, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves an employee by id.
        /// </summary>
        [HttpGet("GetEmployeeById/{userId}")]
        public async Task<ActionResult> GetEmployeeById(string userId)
        {
            _logger.LogInformation("Retrieving employee with id {UserId}", userId);

            try
            {
                Employee? employee = await _employeeRepository.GetEmployeeById(userId);

                if (employee is null)
                {
                    _logger.LogWarning("Employee with id {UserId} was not found", userId);
                    return NotFound($"Employee with ID '{userId}' was not found");
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to retrieve employee {UserId}: {Message}", userId, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all employees affiliated with a specific gym.
        /// </summary>
        [HttpGet("GetEmployeeByAffiliation/{affiliationId}")]
        public async Task<ActionResult> GetEmployeeByAffiliation(Guid affiliationId)
        {
            _logger.LogInformation("Retrieving employees for affiliation {AffiliationId}", affiliationId);

            try
            {
                var employees = await _employeeRepository.GetEmployeesByAffiliation(affiliationId);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to retrieve employees for affiliation {AffiliationId}: {Message}", affiliationId, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}