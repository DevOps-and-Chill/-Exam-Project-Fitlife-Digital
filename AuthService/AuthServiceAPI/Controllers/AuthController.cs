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
            _logger.LogInformation("Registrerer nye credentials for bruger");
            try
            {
                await _credentialService.CreateCredential(registerDTO);
                _logger.LogInformation("Credentials oprettet succesfuldt");
                return Ok(new
                {
                    message = "Credential created successfully"
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Ugyldig registreringsanmodning: {message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            _logger.LogInformation("Login forsøg");
            try
            {
                string? token = await _credentialService.Login(loginRequestDTO);

                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("Login mislykkedes — ugyldige credentials");
                    return Unauthorized();
                }

                _logger.LogInformation("Login succesfuldt");
                return Ok(new
                {
                    token
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Ugyldig login anmodning: {message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("DeleteCredentials")]
        public async Task<ActionResult> DeleteCredentials([FromBody] string userId)
        {
            _logger.LogInformation("Sletter credentials for bruger: {userId}", userId);
            try
            {
                await _credentialService.RemoveCredentials(userId);
                _logger.LogInformation("Credentials slettet for bruger: {userId}", userId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Credentials ikke fundet for bruger: {userId}", userId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Fejl ved sletning af credentials for bruger: {userId} - {message}", userId, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
