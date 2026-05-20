using AuthServiceAPI.DTOs;
using AuthServiceAPI.Models;
using AuthServiceAPI.Repositories.Interfaces;
using AuthServiceAPI.Services.Interfaces;
using System.Net;

namespace AuthServiceAPI.Services
{
    public class CredentialService : ICredentialService
    {
        private readonly IPasswordService _passwordService;
        private readonly ICredentialRepository _authRepository;

        public CredentialService(IPasswordService passwordService, ICredentialRepository authRepository)
        {
            _passwordService = passwordService;
            _authRepository = authRepository;
        }

        public async Task CreateCredential(RegisterCredentialsRequestDTO credentialRequest)
        {
            Credential? existingCredential = await _authRepository.GetCredentialsByEmail(credentialRequest.Email);

            if (existingCredential != null)
            {
                throw new ArgumentException("Email already exists.");
            }

            if (string.IsNullOrWhiteSpace(credentialRequest.Password))
            {
                throw new ArgumentException("Password is required. Cannot be null or empty/whitespace");
            }
            string hashedPassword = _passwordService.HashPassword(credentialRequest.Password);

            Credential newCredential = new Credential(
                credentialRequest.UserId,
                credentialRequest.Email,
                hashedPassword);

            bool result = await _authRepository.RegisterCredentials(newCredential);
            if (!result)
            {
                throw new Exception("Could not create credential"); 
            }
        }

        public async Task<bool> ValidateCredential(ValidateCredentialsRequestDTO validationRequest)
        {
            bool result = false;
            if (string.IsNullOrEmpty(validationRequest.Email))
            {
                Credential credentialsToValidate = await _authRepository.GetCredentialsByEmail(validationRequest.Email);
                if (string.IsNullOrWhiteSpace(validationRequest.Password))
                {
                    throw new ArgumentException("Password is required in order to perform validation");
                }
                result = _passwordService.VerifyPassword(validationRequest.Password, credentialsToValidate.PasswordHash);
                if (result)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task RemoveCredentials(string credentialId)
        {

           await _authRepository.RemoveCredentials(credentialId);
                
        }
    }
}
