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

            //
            var hostName = System.Net.Dns.GetHostName();
            var ips = System.Net.Dns.GetHostAddresses(hostName);
            var ipaddr = ips.First().MapToIPv4().ToString();
            _logger.LogInformation(1, $"AuthService responding from {ipaddr}");
        }

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
