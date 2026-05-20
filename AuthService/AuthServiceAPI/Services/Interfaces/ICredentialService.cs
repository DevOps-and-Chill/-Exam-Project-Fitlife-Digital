using AuthServiceAPI.DTOs;
using AuthServiceAPI.Models;

namespace AuthServiceAPI.Services.Interfaces
{
    public interface ICredentialService
    {
        public Task CreateCredential(RegisterCredentialsRequestDTO credentialRequest);
        public Task<bool> ValidateCredential(LoginRequestDTO validationRequest);
        public Task RemoveCredentials(string credentialId);
        public Task<Credential> GetCredentialsByEmail(string email);
        public Task<string?> Login(LoginRequestDTO loginRequest);
    }
}
