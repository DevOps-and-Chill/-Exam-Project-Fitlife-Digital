using Microsoft.AspNetCore.Mvc;
using UserServiceAPI.Models;
using UserServiceAPI.Repositories;
using UserServiceAPI.Repositories.Interfaces;

namespace UserServiceAPI.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                return Ok(await _userRepository.GetAllUsers());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetByAffiliation")]
        public async Task<ActionResult> GetUsersByAffiliation(Guid exerciseGymId)
        {
            try
            {
                return Ok(await _userRepository.GetUsersByAffiliation(exerciseGymId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetUsersInGymByRole")]
        public async Task<ActionResult> GetUsersInExerciseGymByRole(Guid exerciseGymId, string role)
        {
            try
            {
                return Ok(await _userRepository.GetUsersInExerciseGymByRole(exerciseGymId, role));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("addtestdata")]
        public async Task<ActionResult> AddData()
        {
            try
            {
                var result = await _userRepository.LoadTestData();
                if (result)
                {
                    return Ok();
                }
                else
                    return Ok("error in loading data");
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
