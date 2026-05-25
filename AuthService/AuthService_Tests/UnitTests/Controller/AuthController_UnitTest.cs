using AuthServiceAPI.Controllers;
using AuthServiceAPI.DTOs;
using AuthServiceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Timers;

namespace AuthServiceTests.UnitTests.Controller
{
    [TestClass]
    public class AuthControllerTests
    {
        private Mock<ICredentialService> _credentialServiceMock = null!;
        private Mock<ILogger<AuthController>> _loggerMock = null!;

        private AuthController _controller = null!;

        [TestInitialize]
        public void Setup()
        {
            _credentialServiceMock = new Mock<ICredentialService>();
            _loggerMock = new Mock<ILogger<AuthController>>();

            _controller = new AuthController(
                _credentialServiceMock.Object,
                _loggerMock.Object);
        }

        [TestMethod]
        public async Task RegisterCredentials_ReturnsOk_WhenCredentialCreated()
        {
            // Arrange
            RegisterCredentialsRequestDTO dto = new()
            {
                UserId = Guid.NewGuid().ToString(),
                Email = "test@test.com",
                Password = "Password123"
            };

            _credentialServiceMock
                .Setup(x => x.CreateCredential(dto))
                .Returns(Task.CompletedTask);

            // Act
            ActionResult result =
                await _controller.RegisterCredentials(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            _credentialServiceMock.Verify(
                x => x.CreateCredential(dto),
                Times.Once);
        }

        [TestMethod]
        public async Task RegisterCredentials_ReturnsBadRequest_WhenArgumentExceptionThrown()
        {
            // Arrange
            RegisterCredentialsRequestDTO dto = new()
            {
                UserId = Guid.NewGuid().ToString(),
                Email = "test@test.com",
                Password = ""
            };

            _credentialServiceMock
                .Setup(x => x.CreateCredential(dto))
                .ThrowsAsync(new ArgumentException());

            // Act
            ActionResult result =
                await _controller.RegisterCredentials(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Login_ReturnsOk_WhenLoginSucceeds()
        {
            // Arrange
            LoginRequestDTO dto = new()
            {
                Email = "test@test.com",
                Password = "Password123"
            };

            _credentialServiceMock
                .Setup(x => x.Login(dto))
                .ReturnsAsync("jwt-token");

            // Act
            ActionResult result =
                await _controller.Login(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            _credentialServiceMock.Verify(
                x => x.Login(dto),
                Times.Once);
        }

        [TestMethod]
        public async Task Login_ReturnsUnauthorized_WhenTokenIsNull()
        {
            // Arrange
            LoginRequestDTO dto = new()
            {
                Email = "test@test.com",
                Password = "WrongPassword"
            };

            _credentialServiceMock
                .Setup(x => x.Login(dto))
                .ReturnsAsync((string?)null);

            // Act
            ActionResult result =
                await _controller.Login(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public async Task Login_ReturnsUnauthorized_WhenTokenIsEmpty()
        {
            // Arrange
            LoginRequestDTO dto = new()
            {
                Email = "test@test.com",
                Password = "WrongPassword"
            };

            _credentialServiceMock
                .Setup(x => x.Login(dto))
                .ReturnsAsync(string.Empty);

            // Act
            ActionResult result =
                await _controller.Login(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public async Task Login_ReturnsBadRequest_WhenArgumentExceptionThrown()
        {
            // Arrange
            LoginRequestDTO dto = new()
            {
                Email = "test@test.com",
                Password = ""
            };

            _credentialServiceMock
                .Setup(x => x.Login(dto))
                .ThrowsAsync(new ArgumentException());

            // Act
            ActionResult result =
                await _controller.Login(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task DeleteCredentials_ReturnsOk_WhenCredentialsDeleted()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();

            _credentialServiceMock
                .Setup(x => x.RemoveCredentials(userId))
                .Returns(Task.CompletedTask);

            // Act
            ActionResult result =
                await _controller.DeleteCredentials(userId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));

            _credentialServiceMock.Verify(
                x => x.RemoveCredentials(userId),
                Times.Once);
        }

        [TestMethod]
        public async Task DeleteCredentials_ReturnsNotFound_WhenCredentialDoesNotExist()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();

            _credentialServiceMock
                .Setup(x => x.RemoveCredentials(userId))
                .ThrowsAsync(new KeyNotFoundException());

            // Act
            ActionResult result =
                await _controller.DeleteCredentials(userId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task DeleteCredentials_ReturnsBadRequest_WhenUnexpectedExceptionThrown()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();

            _credentialServiceMock
                .Setup(x => x.RemoveCredentials(userId))
                .ThrowsAsync(new Exception());

            // Act
            ActionResult result =
                await _controller.DeleteCredentials(userId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}