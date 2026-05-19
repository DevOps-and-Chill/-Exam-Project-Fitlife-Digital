using AuthServiceAPI.Models;

namespace AuthServiceAPI.Repositories.Interfaces
{
    public interface ICredentialRepository
    {
        Task<bool> RegisterCredentials(Credential credentialToRegister);
        Task<Credential> GetCredentials(string credentialId);
        Task<Credential> GetCredentialsByEmail(string email);
        Task RemoveCredentials(string credentialId);

    }
}
