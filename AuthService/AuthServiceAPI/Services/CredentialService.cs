using AuthServiceAPI.DTOs;
using AuthServiceAPI.Models;
using AuthServiceAPI.Repositories.Interfaces;
using AuthServiceAPI.Services.Interfaces;
using System.Net;

namespace AuthServiceAPI.Services
{
    /// <summary>
    /// Provides functionality for managing user credentials, authentication and token generation.
    /// </summary>
    public class CredentialService : ICredentialService
    {
        private readonly IPasswordService _passwordService;
        private readonly ICredentialRepository _authRepository;
        private readonly IJWTService _jwtService;
        private readonly ILogger<CredentialService> _logger;

        public CredentialService(
            IPasswordService passwordService,
            ICredentialRepository authRepository,
            IJWTService jwtService,
            ILogger<CredentialService> logger)
        {
            _passwordService = passwordService;
            _authRepository = authRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        /// <summary>
        /// Creates and stores credentials for a new user.
        /// </summary>
        /// <param name="credentialRequest">
        /// Contains the user id, email and password used to create credentials.
        /// </param>
        /// <returns>
        /// A completed task when the credentials have been created successfully.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the email already exists or the password is invalid.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown if the credentials could not be stored.
        /// </exception>
        public async Task CreateCredential(RegisterCredentialsRequestDTO credentialRequest)
        {
            _logger.LogInformation("Creating credentials for user {UserId}", credentialRequest.UserId);

            Credential? existingCredential = await _authRepository.GetCredentialsByEmail(credentialRequest.Email);

            if (existingCredential != null)
            {
                _logger.LogWarning("Credential creation failed because email {Email} already exists", credentialRequest.Email);

                throw new ArgumentException("Email already exists.");
            }

            if (string.IsNullOrWhiteSpace(credentialRequest.Password))
            {
                _logger.LogWarning("Credential creation failed due to missing password");

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
                _logger.LogError("Credential creation failed for user {UserId}", credentialRequest.UserId);

                throw new Exception("Could not create credential");
            }

            _logger.LogInformation("Credentials created successfully for user {UserId}", credentialRequest.UserId);
        }

        /// <summary>
        /// Validates whether the provided login credentials are correct.
        /// </summary>
        /// <param name="validationRequest">
        /// Contains the email and password used for validation.
        /// </param>
        /// <returns>
        /// Returns true if the credentials are valid; otherwise false.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the password is missing.
        /// </exception>
        public async Task<bool> ValidateCredential(LoginRequestDTO validationRequest)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(validationRequest.Email))
            {
                Credential credentialsToValidate = await _authRepository.GetCredentialsByEmail(validationRequest.Email);

                if (string.IsNullOrWhiteSpace(validationRequest.Password))
                {
                    _logger.LogWarning("Credential validation failed due to missing password");

                    throw new ArgumentException(
                        "Password is required in order to perform validation");
                }

                result = _passwordService.VerifyPassword(
                    validationRequest.Password,
                    credentialsToValidate.PasswordHash);

                if (result)
                {
                    _logger.LogInformation("Credential validation succeeded for email {Email}", validationRequest.Email);

                    return true;
                }
            }

            _logger.LogWarning("Credential validation failed");

            return false;
        }

        /// <summary>
        /// Removes credentials associated with the specified user.
        /// </summary>
        /// <param name="credentialId">
        /// The identifier of the credentials to remove.
        /// </param>
        /// <returns>
        /// A completed task when the credentials have been removed.
        /// </returns>
        public async Task RemoveCredentials(string credentialId)
        {
            _logger.LogInformation("Removing credentials for user {UserId}", credentialId);

            await _authRepository.RemoveCredentials(credentialId);

            _logger.LogInformation( "Credentials removed for user {UserId}", credentialId);
        }

        /// <summary>
        /// Retrieves credentials associated with the specified email address.
        /// </summary>
        /// <param name="email">
        /// The email address used to locate credentials.
        /// </param>
        /// <returns>
        /// Returns the matching credential object.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if no credentials exist for the provided email.
        /// </exception>
        public async Task<Credential> GetCredentialsByEmail(string email)
        {
            Credential credential =
                await _authRepository.GetCredentialsByEmail(email);

            if (credential == null)
            {
                _logger.LogWarning("Credentials not found for email {Email}", email);

                throw new KeyNotFoundException( $"usercredentials not found with email {email}");
            }

            return credential;
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <param name="loginRequest">
        /// Contains the email and password used for authentication.
        /// </param>
        /// <returns>
        /// Returns a JWT token if authentication succeeds;
        /// otherwise returns null.
        /// </returns>
        public async Task<string?> Login(LoginRequestDTO loginRequest)
        {
            _logger.LogInformation("Authenticating user with email {Email}", loginRequest.Email);

            Credential credential =
                await _authRepository.GetCredentialsByEmail(loginRequest.Email);

            if (credential == null)
            {
                _logger.LogWarning("Authentication failed because credentials were not found");

                return null;
            }

            bool validPassword = await ValidateCredential(loginRequest);

            if (!validPassword)
            {
                _logger.LogWarning("Authentication failed due to invalid credentials");

                return null;
            }

            _logger.LogInformation("Authentication succeeded for user {UserId}", credential.Id);

            return _jwtService.GenerateToken(credential);
        }
    }
}