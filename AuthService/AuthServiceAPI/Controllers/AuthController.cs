using AuthServiceAPI.DTOs;
using AuthServiceAPI.Models;
using AuthServiceAPI.Repositories.Interfaces;
using AuthServiceAPI.Services;
using AuthServiceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AuthServiceAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly ICredentialService _credentialService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ICredentialService credentialService, ILogger<AuthController> logger)
        {
            _credentialService = credentialService;
            _logger = logger;

            var hostName = System.Net.Dns.GetHostName();
            var ips = System.Net.Dns.GetHostAddresses(hostName);
            var ipaddr = ips.First().MapToIPv4().ToString();

            _logger.LogInformation("AuthService responding from {IpAddress}", ipaddr);
        }

        /// <summary>
        /// Registers credentials for a new user.
        /// </summary>
        /// <param name="registerDTO">
        /// Contains the information required to create credentials.
        /// </param>
        /// <returns>
        /// Returns 200 (OK) when credentials are created successfully.
        /// Returns 400 (BadRequest) if the registration request is invalid.
        /// </returns>
        [AllowAnonymous]
        [HttpPost("RegisterCredentials")]
        public async Task<ActionResult> RegisterCredentials([FromBody] RegisterCredentialsRequestDTO registerDTO)
        {
            _logger.LogDebug("Registering new credentials for user");
            try
            {
                await _credentialService.CreateCredential(registerDTO);
                _logger.LogInformation("Credentials created successfully");
                return Ok(new
                {
                    message = "Credential created successfully"
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid registration request: {message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Authenticates a user and returns an JWT.
        /// </summary>
        /// <param name="loginRequestDTO">
        /// Contains the login credentials used for authentication.
        /// </param>
        /// <returns>
        /// Returns 200 (OK) with a JWT token when authentication succeeds.
        /// Returns 401 (Unauthorized) if the credentials are invalid.
        /// Returns 400 (BadRequest) if the login request is invalid.
        /// </returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            _logger.LogDebug("Login attempt");
            try
            {
                string? token = await _credentialService.Login(loginRequestDTO);

                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("Login failed — invalid credentials");
                    return Unauthorized();
                }

                _logger.LogInformation("Login successful");
                return Ok(new
                {
                    token
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid login request: {message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes credentials for the specified user.
        /// </summary>
        /// <remarks>
        /// Requires authorization.
        /// Removes stored credentials associated with the provided user id.
        /// </remarks>
        /// <param name="userId">
        /// The unique identifier of the user whose credentials should be deleted.
        /// </param>
        /// <returns>
        /// Returns 200 (OK) when credentials are deleted successfully.
        /// Returns 404 (NotFound) if no credentials exist for the specified user.
        /// Returns 400 (BadRequest) if an unexpected error occurs.
        /// </returns>
        [Authorize]
        [HttpDelete("DeleteCredentials")]
        public async Task<ActionResult> DeleteCredentials([FromBody] string userId)
        {
            _logger.LogDebug("Deleting credentials for user: {userId}", userId);
            try
            {
                await _credentialService.RemoveCredentials(userId);
                _logger.LogInformation("Credentials deleted for user: {userId}", userId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Credentials not found for user: {userId}", userId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting credentials for user: {userId} - {message}", userId, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}