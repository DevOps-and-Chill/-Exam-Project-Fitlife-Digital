using AuthServiceAPI.Data;
using AuthServiceAPI.Models;
using AuthServiceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthServiceAPI.Repositories
{
    public class CredentialRepositoryDB : ICredentialRepository
    {
        private readonly CredentialDbContext _context;

        public CredentialRepositoryDB(CredentialDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Registers credentials for a user.
        /// </summary>
        /// <param name="credentialToRegister">
        /// The credential object containing the information to persist.
        /// </param>
        /// <returns>
        /// Returns true when the credentials are successfully stored.
        /// </returns>
        public async Task<bool> RegisterCredentials(Credential credentialToRegister)
        {
            _context.UserCredential.Add(credentialToRegister);

            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Removes credentials associated with the specified user.
        /// </summary>
        /// <param name="userId">
        /// The unique identifier of the user whose credentials should be removed.
        /// </param>
        /// <returns>
        /// A completed task when the credentials have been removed successfully.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when no credentials exist for the specified user.
        /// </exception>
        public async Task RemoveCredentials(string userId)
        {
            Credential? credentialToRemove = await GetCredentials(userId);

            if (credentialToRemove == null)
            {
                throw new KeyNotFoundException(
                    $"Credentials for user with ID {userId} not found");
            }

            _context.UserCredential.Remove(credentialToRemove);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves credentials by credential identifier.
        /// </summary>
        /// <param name="credentialId">
        /// The unique identifier of the credential.
        /// </param>
        /// <returns>
        /// Returns the credential object if found; otherwise null.
        /// </returns>
        public async Task<Credential?> GetCredentials(string credentialId)
        {
            return await _context.UserCredential
                .SingleOrDefaultAsync(c => c.Id == credentialId);
        }

        /// <summary>
        /// Retrieves credentials associated with the specified email address.
        /// </summary>
        /// <param name="email">
        /// The email address used to locate credentials.
        /// </param>
        /// <returns>
        /// Returns the credential object if found.
        /// </returns>
        public async Task<Credential?> GetCredentialsByEmail(string email)
        {
            return await _context.UserCredential
                .SingleOrDefaultAsync(c => c.Email == email.ToLower());
        }
    }
}