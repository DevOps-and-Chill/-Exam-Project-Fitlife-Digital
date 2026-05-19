using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
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

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>
        /// Returns a list of all users.
        /// </returns>
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

        /// <summary>
        /// Retrieves a user by id.
        /// </summary>
        /// <param name="userId">
        /// The id of the user.
        /// </param>
        /// <returns>
        /// Returns the user object if found.
        /// </returns>
        [HttpGet("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            try
            {
                return Ok(await _userRepository.GetUserById(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Retrieves a user id based on email.
        /// </summary>
        /// <param name="email">
        /// The email address of the user.
        /// </param>
        /// <returns>
        /// Returns the user id if a matching user is found.
        /// Returns NotFound if no user exists with the provided email.
        /// </returns>
        [HttpGet("GetUserIdByEmail/{email}")]
        public async Task<IActionResult> GetUserIdByEmail(string email)
        {
            try
            {
                string? userId = await _userRepository.GetUserIdByEmail(email);

                if (string.IsNullOrWhiteSpace(userId))
                {
                    return NotFound();
                }
                else
                {
                    return Ok(userId);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Loads test data into the database.
        /// </summary>
        /// <returns>
        /// Returns Ok if the test data was loaded successfully.
        /// </returns>
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
                {
                    return Ok("error in loading data");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
