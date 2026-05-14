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
        public async Task<ActionResult> EndEmployment(Guid userId)
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
        public async Task<ActionResult> SetEmployeeAsManager(Guid userId)
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

    }
}
