using AuthServiceAPI.DTOs;
using AuthServiceAPI.Models;
using AuthServiceAPI.Repositories.Interfaces;
using AuthServiceAPI.Services;
using AuthServiceAPI.Services.Interfaces;
using Moq;

namespace AuthService_Tests.UnitTests.Services
{
    [TestClass]
    public class CredentialServiceTests
    {
        private Mock<IPasswordService> _passwordServiceMock = null!;
        private Mock<ICredentialRepository> _repositoryMock = null!;
        private Mock<IJWTService> _jwtServiceMock = null!;
        private Mock<Microsoft.Extensions.Logging.ILogger<CredentialService>> _loggerMock = null!;

        private CredentialService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _passwordServiceMock = new Mock<IPasswordService>();
            _repositoryMock = new Mock<ICredentialRepository>();
            _jwtServiceMock = new Mock<IJWTService>();

            _loggerMock =
                new Mock<Microsoft.Extensions.Logging.ILogger<CredentialService>>();

            _service = new CredentialService(
                _passwordServiceMock.Object,
                _repositoryMock.Object,
                _jwtServiceMock.Object,
                _loggerMock.Object);
        }

        [TestMethod]
        public async Task CreateCredential_CreatesCredential_WhenInputIsValid()
        {
            // Arrange
            RegisterCredentialsRequestDTO dto = new()
            {
                UserId = Guid.NewGuid().ToString(),
                Email = "test@test.com",
                Password = "Password123"
            };

            _repositoryMock
                .Setup(x => x.GetCredentialsByEmail(dto.Email))
                .ReturnsAsync((Credential)null!);

            _passwordServiceMock
                .Setup(x => x.HashPassword(dto.Password))
                .Returns("hashed-password");

            _repositoryMock
                .Setup(x => x.RegisterCredentials(It.IsAny<Credential>()))
                .ReturnsAsync(true);

            // Act
            await _service.CreateCredential(dto);

            // Assert
            _passwordServiceMock.Verify(
                x => x.HashPassword(dto.Password),
                Times.Once);

            _repositoryMock.Verify(
                x => x.RegisterCredentials(It.IsAny<Credential>()),
                Times.Once);
        }

        [TestMethod]
        public async Task CreateCredential_ThrowsArgumentException_WhenEmailExists()
        {
            // Arrange
            RegisterCredentialsRequestDTO dto = new()
            {
                UserId = "1",
                Email = "test@test.com",
                Password = "Password123"
            };

            _repositoryMock
                .Setup(x => x.GetCredentialsByEmail(dto.Email))
                .ReturnsAsync(
                    new Credential(
                        dto.UserId,
                        dto.Email,
                        "hash"));

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => _service.CreateCredential(dto));
        }

        [TestMethod]
        public async Task CreateCredential_ThrowsArgumentException_WhenPasswordIsEmpty()
        {
            // Arrange
            RegisterCredentialsRequestDTO dto = new()
            {
                UserId = "1",
                Email = "test@test.com",
                Password = ""
            };

            _repositoryMock
                .Setup(x => x.GetCredentialsByEmail(dto.Email))
                .ReturnsAsync((Credential)null!);

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => _service.CreateCredential(dto));
        }

        [TestMethod]
        public async Task CreateCredential_ThrowsException_WhenRepositoryFails()
        {
            // Arrange
            RegisterCredentialsRequestDTO dto = new()
            {
                UserId = "1",
                Email = "test@test.com",
                Password = "Password123"
            };

            _repositoryMock
                .Setup(x => x.GetCredentialsByEmail(dto.Email))
                .ReturnsAsync((Credential)null!);

            _passwordServiceMock
                .Setup(x => x.HashPassword(dto.Password))
                .Returns("hash");

            _repositoryMock
                .Setup(x => x.RegisterCredentials(It.IsAny<Credential>()))
                .ReturnsAsync(false);

            // Act + Assert
            await Assert.ThrowsAsync<Exception>(
                () => _service.CreateCredential(dto));
        }

        [TestMethod]
        public async Task ValidateCredential_ReturnsTrue_WhenPasswordMatches()
        {
            // Arrange
            LoginRequestDTO dto = new()
            {
                Email = "test@test.com",
                Password = "Password123"
            };

            Credential credential =
                new("1", dto.Email, "hash");

            _repositoryMock
                .Setup(x => x.GetCredentialsByEmail(dto.Email))
                .ReturnsAsync(credential);

            _passwordServiceMock
                .Setup(x => x.VerifyPassword(
                    dto.Password,
                    credential.PasswordHash))
                .Returns(true);

            // Act
            bool result =
                await _service.ValidateCredential(dto);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ValidateCredential_ReturnsFalse_WhenPasswordIsInvalid()
        {
            // Arrange
            LoginRequestDTO dto = new()
            {
                Email = "test@test.com",
                Password = "WrongPassword"
            };

            Credential credential =
                new("1", dto.Email, "hash");

            _repositoryMock
                .Setup(x => x.GetCredentialsByEmail(dto.Email))
                .ReturnsAsync(credential);

            _passwordServiceMock
                .Setup(x => x.VerifyPassword(
                    dto.Password,
                    credential.PasswordHash))
                .Returns(false);

            // Act
            bool result =
                await _service.ValidateCredential(dto);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task ValidateCredential_ThrowsArgumentException_WhenPasswordMissing()
        {
            // Arrange
            LoginRequestDTO dto = new()
            {
                Email = "test@test.com",
                Password = ""
            };

            _repositoryMock
                .Setup(x => x.GetCredentialsByEmail(dto.Email))
                .ReturnsAsync(
                    new Credential(
                        "1",
                        dto.Email,
                        "hash"));

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => _service.ValidateCredential(dto));
        }

        [TestMethod]
        public async Task GetCredentialsByEmail_ReturnsCredential_WhenFound()
        {
            // Arrange
            Credential credential =
                new("1", "test@test.com", "hash");

            _repositoryMock
                .Setup(x => x.GetCredentialsByEmail(
                    credential.Email))
                .ReturnsAsync(credential);

            // Act
            Credential result =
                await _service.GetCredentialsByEmail(
                    credential.Email);

            // Assert
            Assert.AreEqual(
                credential.Email,
                result.Email);
        }

        [TestMethod]
        public async Task GetCredentialsByEmail_ThrowsKeyNotFoundException_WhenMissing()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetCredentialsByEmail(
                    It.IsAny<string>()))
                .ReturnsAsync((Credential)null!);

            // Act + Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.GetCredentialsByEmail(
                    "missing@test.com"));
        }

        [TestMethod]
        public async Task Login_ReturnsToken_WhenAuthenticationSucceeds()
        {
            // Arrange
            LoginRequestDTO dto = new()
            {
                Email = "test@test.com",
                Password = "Password123"
            };

            Credential credential =
                new("1", dto.Email, "hash");

            _repositoryMock
                .Setup(x => x.GetCredentialsByEmail(dto.Email))
                .ReturnsAsync(credential);

            _passwordServiceMock
                .Setup(x => x.VerifyPassword(
                    dto.Password,
                    credential.PasswordHash))
                .Returns(true);

            _jwtServiceMock
                .Setup(x => x.GenerateToken(credential))
                .Returns("jwt-token");

            // Act
            string? result =
                await _service.Login(dto);

            // Assert
            Assert.AreEqual(
                "jwt-token",
                result);
        }

        [TestMethod]
        public async Task Login_ReturnsNull_WhenCredentialMissing()
        {
            // Arrange
            LoginRequestDTO dto = new()
            {
                Email = "missing@test.com",
                Password = "Password123"
            };

            _repositoryMock
                .Setup(x => x.GetCredentialsByEmail(dto.Email))
                .ReturnsAsync((Credential)null!);

            // Act
            string? result =
                await _service.Login(dto);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Login_ReturnsNull_WhenPasswordInvalid()
        {
            // Arrange
            LoginRequestDTO dto = new()
            {
                Email = "test@test.com",
                Password = "wrong"
            };

            Credential credential =
                new("1", dto.Email, "hash");

            _repositoryMock
                .Setup(x => x.GetCredentialsByEmail(dto.Email))
                .ReturnsAsync(credential);

            _passwordServiceMock
                .Setup(x => x.VerifyPassword(
                    dto.Password,
                    credential.PasswordHash))
                .Returns(false);

            // Act
            string? result =
                await _service.Login(dto);

            // Assert
            Assert.IsNull(result);
        }
    }
}