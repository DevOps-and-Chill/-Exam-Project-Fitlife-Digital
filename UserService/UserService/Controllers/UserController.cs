using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;

            // Log which server and IP is responding
            var hostName = System.Net.Dns.GetHostName();
            var ips = System.Net.Dns.GetHostAddresses(hostName);
            var ipaddr = ips.First().MapToIPv4().ToString();

            _logger.LogInformation("UserService responding from {IpAddress}", ipaddr);
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>
        /// Returns a list of all users.
        /// </returns>
        [AllowAnonymous]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogDebug("Fetching all users");
            try
            {
                return Ok(await _userRepository.GetAllUsers());
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching all users: {message}", ex.Message);
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
        [AllowAnonymous]
        [HttpGet("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            
            _logger.LogDebug("Fetching user with id: {userId}", userId);
            try
            {
                return Ok(await _userRepository.GetUserById(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching user with id {userId}: {message}", userId, ex.Message);
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
        [AllowAnonymous]
        [HttpGet("GetUserIdByEmail/{email}")]
        public async Task<IActionResult> GetUserIdByEmail(string email)
        {
            _logger.LogDebug("Fetching user id for email: {email}", email);
            try
            {
                string? userId = await _userRepository.GetUserIdByEmail(email);

                if (string.IsNullOrWhiteSpace(userId))
                {
                    _logger.LogWarning("No user found with email: {email}", email);
                    return NotFound();
                }

                return Ok(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching user id for email {email}: {message}", email, ex.Message);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Loads test data into the database.
        /// </summary>
        /// <returns>
        /// Returns Ok if the test data was loaded successfully.
        /// </returns>
        [Authorize]
        [HttpGet("addtestdata")]
        public async Task<ActionResult> AddData()
        {
            _logger.LogDebug("Loading test data");
            try
            {
                var result = await _userRepository.LoadTestData();

                if (result)
                {
                    _logger.LogInformation("Test data loaded successfully");
                    return Ok();
                }
                else
                {
                    _logger.LogWarning("Error loading test data");
                    return Ok("error in loading data");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error loading test data: {message}", ex.Message);
                return BadRequest(ex);
            }
        }
    }
}