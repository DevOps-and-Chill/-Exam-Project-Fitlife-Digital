using AuthServiceAPI.DTOs;
using AuthServiceAPI.Models;

namespace AuthServiceAPI.Services.Interfaces
{
    public interface ICredentialService
    {
        public Task CreateCredential(RegisterCredentialsRequestDTO credentialRequest);
        public Task<bool> ValidateCredential(ValidateCredentialsRequestDTO validationRequest);
        Task RemoveCredentials(string credentialId);
    }
}
