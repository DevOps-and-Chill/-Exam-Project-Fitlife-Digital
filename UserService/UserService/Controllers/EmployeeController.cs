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
        /// <remarks>Exposed as an HTTP PUT endpoint at 'EndEmploymentForEmployee/{userId}'. The operation
        /// is performed asynchronously via the employee repository.</remarks>
        /// <param name="userId">The identifier of the employee whose employment is to be ended.</param>
        /// <returns>An ActionResult containing the repository result; returns 200 OK with the result on success or 400 Bad
        /// Request with the exception on failure.</returns>
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
        /// <param name="userId">The identifier of the employee to promote to manager.</param>
        /// <returns>An ActionResult containing the repository result on success (HTTP 200) or a BadRequest with the exception on
        /// failure.</returns>
        [HttpPut("SetEmployeeAsManager/{userId}")]
        public async Task<ActionResult> SetEmployeeAsManager(string userId)
        {
            _logger.LogDebug("Setting employee {userId} as manager", userId);
            try
            {
                return Ok(await _employeeRepository.SetEmployeeAsManager(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error promoting employee {userId}: {message}", userId, ex.Message);
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// Upserts the specified employee and returns the created or updated employee.
        /// </summary>
        /// <remarks>Performs the operation asynchronously via the employee repository.
        /// InvalidOperationException is translated to a 400 BadRequest containing the exception message.</remarks>
        /// <param name="employee">Employee to create or update; an existing employee is updated when an identifier is present.</param>
        /// <returns>An ActionResult containing the upserted Employee (200 OK) on success, or a 400 BadRequest with an error
        /// message if the operation is invalid.</returns>
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
        /// <remarks>Invoked via HTTP DELETE at DeleteEmployee/{userId}. Executes
        /// asynchronously.</remarks>
        /// <param name="userId">The unique identifier of the employee to delete.</param>
        /// <returns>An ActionResult containing the deletion result: 200 (OK) with the deletion result on success, or 400
        /// (BadRequest) with an error message on failure.</returns>
        [HttpDelete("DeleteEmployee/{userId}")]
        public async Task<ActionResult> DeleteEmployee(string userId)
        {
            _logger.LogDebug("Deleting employee: {userId}", userId);
            try
            {
                return Ok(await _employeeRepository.DeleteEmployee(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting employee {userId}: {message}", userId, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary> Sets an employee account as inactive.</summary>
        /// <param name="userId">The id of the employee account. </param>
        /// <returns> Returns the updated employee object with ActiveUser set to false. </returns>
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
        /// <param name="userId">
        /// The id of the employee.
        /// </param>
        /// <returns>
        /// Returns the employee object if found.
        /// Returns NotFound if no employee exists with the provided id.
        /// </returns>
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
        /// <param name="affiliationId">
        /// The affiliation id of the gym.
        /// </param>
        /// <returns>
        /// Returns a list of employees associated with the provided affiliation id.
        /// </returns>
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
