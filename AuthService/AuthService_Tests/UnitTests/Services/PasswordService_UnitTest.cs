using AuthServiceAPI.Services;

namespace AuthService_Tests.UnitTests.Services
{
    [TestClass]
    public class PasswordServiceTests
    {
        private PasswordService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _service = new PasswordService();
        }

        [TestMethod]
        public void HashPassword_ReturnsHashedValue()
        {
            // Arrange
            string password = "Password123";

            // Act
            string result =
                _service.HashPassword(password);

            // Assert
            Assert.IsFalse(
                string.IsNullOrWhiteSpace(result));

            Assert.AreNotEqual(
                password,
                result);
        }

        [TestMethod]
        public void VerifyPassword_ReturnsTrue_WhenPasswordMatches()
        {
            // Arrange
            string password = "Password123";

            string hash =
                _service.HashPassword(password);

            // Act
            bool result =
                _service.VerifyPassword(
                    password,
                    hash);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void VerifyPassword_ReturnsFalse_WhenPasswordDoesNotMatch()
        {
            // Arrange
            string password = "Password123";

            string hash =
                _service.HashPassword(password);

            // Act
            bool result =
                _service.VerifyPassword(
                    "WrongPassword",
                    hash);

            // Assert
            Assert.IsFalse(result);
        }
    }
}