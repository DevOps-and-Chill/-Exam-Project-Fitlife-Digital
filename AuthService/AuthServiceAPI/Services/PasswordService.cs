using AuthServiceAPI.Services.Interfaces;
using BCrypt.Net;

namespace AuthServiceAPI.Services
{
    /// <summary>
    /// AO: Provides functionality for securely hashing and verifying passwords.
    /// Uses BCrypt to generate salted password hashes and validate passwords without storing plain text values.
    /// </summary>
    public class PasswordService : IPasswordService
    {
        /// <summary>
        /// Creates a secure BCrypt hash of the provided password.
        /// The generated hash includes a salt and can be safely stored in the database.
        /// </summary>
        /// <param name="password">
        /// The plain text password to hash.
        /// </param>
        /// <returns>
        /// Returns the generated password hash.
        /// </returns>
        public string HashPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            return hashedPassword;
        }

        /// <summary>
        /// Verifies whether a plain text password matches a previously generated BCrypt hash.
        /// Used during authentication to validate user credentials.
        /// </summary>
        /// <param name="password">
        /// The plain text password to verify.
        /// </param>
        /// <param name="hashedPassword">
        /// The stored BCrypt password hash.
        /// </param>
        /// <returns>
        /// Returns true if the password matches the hash; otherwise false.
        /// </returns>
        public bool VerifyPassword(string password, string hashedPassword)
        {
            bool result = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

            return result;
        }
    }
}