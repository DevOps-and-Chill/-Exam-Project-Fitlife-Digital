using Microsoft.AspNetCore.Mvc;
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

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet("GetAllEmployees")]
        public async Task<ActionResult> GetAllEmployees()
        {
            try
            {
                return Ok(await _employeeRepository.GetAllEmployees());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("EndEmploymentForEmployee")]
        public async Task<ActionResult> EndEmployment(string userId)
        {
            try
            {
                return Ok(await _employeeRepository.EndEmploymentForEmployee(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("SetEmployeeAsManager")]
        public async Task<ActionResult> SetEmployeeAsManager(string userId)
        {
            try
            {
                return Ok(await _employeeRepository.SetEmployeeAsManager(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("UpsertEmployee")]
        public async Task<ActionResult> UpsertEmployee([FromBody] Employee employee)
        {
            try
            {
                return Ok(await _employeeRepository.UpsertEmployee(employee));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteEmployee")]
        public async Task<ActionResult> DeleteEmployee(string userId)
        {
            try
            {
                return Ok(await _employeeRepository.DeleteEmployee(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("SetAccountAsInactive")]
        public async Task<ActionResult> SetEmployeeAccountAsInactive(string userId)
        {
            try
            {
                return Ok(await _employeeRepository.SetAccountAsInactive(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
