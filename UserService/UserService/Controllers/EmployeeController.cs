using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpGet("GetAllEmployees")]
        public async Task<ActionResult> GetAllEmployees()
        {
            _logger.LogDebug("Fetching all employees");
            try
            {
                return Ok(await _employeeRepository.GetAllEmployees());
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching all employees: {message}", ex.Message);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// End an employee's employment by user identifier.
        /// </summary>
        [Authorize]
        [HttpPut("EndEmploymentForEmployee/{userId}")]
        public async Task<ActionResult> EndEmployment(string userId)
        {
            _logger.LogDebug("Ending employment for employee: {userId}", userId);
            try
            {
                return Ok(await _employeeRepository.EndEmploymentForEmployee(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error ending employment for {userId}: {message}", userId, ex.Message);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Sets the specified employee as a manager.
        /// </summary>
        [Authorize]
        [HttpPut("SetEmployeeAsManager/{userId}")]
        public async Task<ActionResult> SetEmployeeAsManager(string userId)
        {
            _logger.LogDebug("Setting employee {userId} as manager", userId);
            try
            {
                var result = await _employeeRepository.SetEmployeeAsManager(userId);
                
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Could not promote employee {userId}: {message}", userId, ex.Message);

                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error promoting employee {userId}", userId);

                return StatusCode(500, "An unexpected error occurred");
            }
        }

        /// <summary>
        /// Upserts the specified employee and returns the created or updated employee.
        /// </summary>
        [Authorize]
        [HttpPost("UpsertEmployee")]
        public async Task<ActionResult> UpsertEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug("Upserting employee: {employeeId}", employee);
            try
            {
                Employee updatedEmployee = await _employeeRepository.UpsertEmployee(employee);
                _logger.LogInformation("Employee upserted: {employeeId}", employee.Id);
                return Ok(updatedEmployee);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid operation upserting employee {employeeId}: {message}", employee.Id, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the employee identified by the provided userId.
        /// </summary>
        [Authorize]
        [HttpDelete("DeleteEmployee/{userId}")]
        public async Task<ActionResult> DeleteEmployee(string userId)
        {
            _logger.LogDebug("Deleting employee: {userId}", userId);
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
                _logger.LogError("Error deleting employee {userId}: {message}", userId, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Sets an employee account as inactive.
        /// </summary>
        [Authorize]
        [HttpPut("SetAccountAsInactive/{userId}")]
        public async Task<ActionResult> SetEmployeeAccountAsInactive(string userId)
        {
            _logger.LogDebug("Setting employee account as inactive: {userId}", userId);
            try
            {
                return Ok(await _employeeRepository.SetAccountAsInactive(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deactivating employee account {userId}: {message}", userId, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves an employee by id.
        /// </summary>
        [Authorize]
        [HttpGet("GetEmployeeById/{userId}")]
        public async Task<ActionResult> GetEmployeeById(string userId)
        {
            _logger.LogDebug("Fetching employee with id: {userId}", userId);
            try
            {
                Employee? employee = await _employeeRepository.GetEmployeeById(userId);

                if (employee is null)
                {
                    _logger.LogWarning("Employee with id {userId} was not found", userId);
                    return NotFound(
                        $"Employee with id '{userId}' was not found");
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching employee {userId}: {message}", userId, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all employees affiliated with a specific gym.
        /// </summary>
        [Authorize]
        [HttpGet("GetEmployeeByAffiliation/{affiliationId}")]
        public async Task<ActionResult> GetEmployeeByAffiliation(Guid affiliationId)
        {
            _logger.LogDebug("Fetching employees for affiliation: {affiliationId}", affiliationId);
            try
            {
                var employees = await _employeeRepository.GetEmployeesByAffiliation(affiliationId);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching employees for affiliation {affiliationId}: {message}", affiliationId, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}