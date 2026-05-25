using AuthServiceAPI.Data;
using AuthServiceAPI.Models;
using AuthServiceAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuthService_Tests.IntegrationTests.Repositories
{
    [TestClass]
    public class CredentialRepositoryDBTests
    {
        private CredentialDbContext _context = null!;
        private CredentialRepositoryDB _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            DbContextOptions<CredentialDbContext> options =
                new DbContextOptionsBuilder<CredentialDbContext>()
                    .UseInMemoryDatabase(
                        Guid.NewGuid().ToString())
                    .Options;

            _context =
                new CredentialDbContext(options);

            _repository =
                new CredentialRepositoryDB(_context);
        }

        [TestMethod]
        public async Task RegisterCredentials_SavesCredential()
        {
            // Arrange
            Credential credential =
                new(
                    "1",
                    "test@test.com",
                    "hash");

            // Act
            bool result =
                await _repository.RegisterCredentials(
                    credential);

            Credential? saved =
                await _context.UserCredential
                    .SingleOrDefaultAsync(
                        x => x.Id == credential.Id);

            // Assert
            Assert.IsTrue(result);

            Assert.IsNotNull(saved);

            Assert.AreEqual(
                credential.Email,
                saved.Email);
        }

        [TestMethod]
        public async Task GetCredentials_ReturnsCredential_WhenFound()
        {
            // Arrange
            Credential credential =
                new(
                    "1",
                    "test@test.com",
                    "hash");

            _context.UserCredential.Add(
                credential);

            await _context.SaveChangesAsync();

            // Act
            Credential? result =
                await _repository.GetCredentials(
                    credential.Id);

            // Assert
            Assert.IsNotNull(result);

            Assert.AreEqual(
                credential.Id,
                result.Id);
        }

        [TestMethod]
        public async Task GetCredentials_ReturnsNull_WhenNotFound()
        {
            // Act
            Credential? result =
                await _repository.GetCredentials(
                    "missing");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetCredentialsByEmail_ReturnsCredential_WhenFound()
        {
            // Arrange
            Credential credential =
                new(
                    "1",
                    "test@test.com",
                    "hash");

            _context.UserCredential.Add(
                credential);

            await _context.SaveChangesAsync();

            // Act
            Credential? result =
                await _repository.GetCredentialsByEmail(
                    credential.Email);

            // Assert
            Assert.IsNotNull(result);

            Assert.AreEqual(
                credential.Email,
                result.Email);
        }

        [TestMethod]
        public async Task GetCredentialsByEmail_ReturnsNull_WhenNotFound()
        {
            // Act
            Credential? result =
                await _repository.GetCredentialsByEmail(
                    "missing@test.com");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task RemoveCredentials_RemovesCredential()
        {
            // Arrange
            Credential credential =
                new(
                    "1",
                    "test@test.com",
                    "hash");

            _context.UserCredential.Add(
                credential);

            await _context.SaveChangesAsync();

            // Act
            await _repository.RemoveCredentials(
                credential.Id);

            Credential? result =
                await _context.UserCredential
                    .SingleOrDefaultAsync(
                        x => x.Id == credential.Id);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task RemoveCredentials_ThrowsKeyNotFoundException_WhenMissing()
        {
            // Act + Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _repository.RemoveCredentials(
                    "missing"));
        }
    }
}