using AuthServiceAPI.DTOs;
using AuthServiceAPI.Models;
using AuthServiceAPI.Repositories.Interfaces;
using AuthServiceAPI.Services;
using AuthServiceAPI.Services.Interfaces;
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

        public AuthController(ICredentialService credentialService)
        {
            _credentialService = credentialService;
        }

        [HttpPost("RegisterCredentials")]
        public async Task<ActionResult> RegisterCredentials([FromBody] RegisterCredentialsRequestDTO registerDTO)
        {
            try
            {
                await _credentialService.CreateCredential(registerDTO);
                return Ok(new
                {
                    message = "Credential created successfully"
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ValidateCredentials")]
        public async Task<ActionResult> ValidateCredentials([FromBody] ValidateCredentialsRequestDTO validateDTO)
        {
            try
            {
                bool result = await _credentialService.ValidateCredential(validateDTO);
                if (result)
                {
                    return Ok(new
                    {
                        message = "Validation successful"
                    });
                }
                else
                {
                    return Unauthorized(new
                    {
                        message = "Validation failed"
                    });
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteCredentials")]
        public async Task<ActionResult> DeleteCredentials([FromBody] string userId)
        {
            try
            {
                await _credentialService.RemoveCredentials(userId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
