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

        public async Task<bool> RegisterCredentials(Credential credentialToRegister)
        {
            _context.UserCredential.Add(credentialToRegister);
            await _context.SaveChangesAsync();
            return true; 
        }

        public async Task RemoveCredentials(string userId)
        {
            Credential? credentialToRemove = await GetCredentials(userId);

            if (credentialToRemove == null)
            {
                throw new KeyNotFoundException($"Credentials for user with ID {userId} not found");
            }

            _context.UserCredential.Remove(credentialToRemove);

            await _context.SaveChangesAsync();
        }

        public async Task<Credential?> GetCredentials(string credentialId)
        {
            return await _context.UserCredential.SingleOrDefaultAsync(c => c.Id == credentialId);
        }
        public async Task<Credential> GetCredentialsByEmail(string email)
        {
            return await _context.UserCredential.SingleOrDefaultAsync(c => c.Email == email);
        }
    }


}
